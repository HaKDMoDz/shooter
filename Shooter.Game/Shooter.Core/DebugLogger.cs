using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter.Core
{
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
}
