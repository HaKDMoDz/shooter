using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter.Core
{
    public class ReactiveWriter : TextWriter
    {
        private BehaviorSubject<string> currentLine;

        public ReactiveWriter()
        {
            this.Logs = new Subject<BehaviorSubject<string>>();
        }

        public Subject<BehaviorSubject<String>> Logs { get; private set; }

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }

        public override void Write(char value)
        {
            if (value == '\n')
            {
                this.currentLine = new BehaviorSubject<string>("");
                this.Logs.OnNext(this.currentLine);
            }

            this.currentLine.OnNext(this.currentLine.First() + value.ToString(CultureInfo.InvariantCulture));
        }
    }

    public class DebugLogger : GameObject
    {
        private readonly List<DebugLog> logs = new List<DebugLog>(); 
        private SpriteBatch spriteBatch;
        private SpriteFont font;

        public DebugLogger(Engine engine)
            : base(engine)
        {
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            disposables.Add(this.spriteBatch = new SpriteBatch(this.Engine.Game.GraphicsDevice));

            this.font = this.Engine.Game.Content.Load<SpriteFont>("font");
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            attachments.Add(this.Engine.Updates.Subscribe(this.Update));
            attachments.Add(this.Engine.Draws.ObserveOn(this.Engine.PostDrawScheduler).Subscribe(this.Draw));
        }

        public void Log(string value)
        {
            this.logs.Add(new DebugLog(value, DateTime.UtcNow + TimeSpan.FromSeconds(1)));
        }

        public void Log(string value, TimeSpan displayTime)
        {
            this.logs.Add(new DebugLog(value, DateTime.UtcNow + displayTime));
        }

        private void Update(EngineTime time)
        {
            this.logs.RemoveAll((x) => DateTime.UtcNow > x.DeletionDate);
        }

        private void Draw(EngineTime time)
        {
            var text = this.logs.Aggregate(new StringBuilder(), (sb, x) => sb.AppendLine(x.Value)).ToString();

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            this.spriteBatch.DrawString(this.font, text, Vector2.One, Color.Black);
            this.spriteBatch.DrawString(this.font, text, Vector2.Zero, Color.White);
            this.spriteBatch.End();
        }
    }

    public class DebugLog
    {
        public readonly string Value;
        public readonly DateTime DeletionDate;

        public DebugLog(string value)
            : this(value, DateTime.UtcNow)
        {
        }

        public DebugLog(string value, DateTime deletionDate)
        {
            this.Value = value;
            this.DeletionDate = deletionDate;
        }
    }
}
