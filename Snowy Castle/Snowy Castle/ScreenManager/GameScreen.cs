using System;
using Microsoft.Xna.Framework;
using System.IO;

namespace Snowy_Castle
{
    public enum screenState
    {
        On,
        Active,
        Off,
        Hidden,
    }

    public abstract class gameScreen
    {
        public bool popsUp
        {
            get 
            { 
                return pop; 
            }
            protected set 
            { 
                pop = value; 
            }
        }
        bool pop = false;

        public TimeSpan onTime
        {
            get 
            { 
                return oT; 
            }
            protected set 
            { 
                oT = value; 
            }
        }        
        TimeSpan oT = TimeSpan.Zero;

        public TimeSpan offTime
        {
            get 
            { 
                return offT; 
            }
            protected set 
            { 
                offT = value; 
            }
        }
        TimeSpan offT = TimeSpan.Zero;

        public float transPos
        {
            get 
            { 
                return pos; 
            }
            protected set 
            { 
                pos = value; 
            }
        }
        float pos = 1;

        public float transAlpha
        {
            get 
            { 
                return 1f - transPos;
            }
        }

        public screenState state
        {
            get 
            { 
                return currentState; 
            }
            protected set 
            { 
                currentState = value; 
            }
        }
        screenState currentState = screenState.On;

        public bool exiting
        {
            get 
            { 
                return exit; 
            }
            protected internal set 
            { 
                exit = value; 
            }
        }
        bool exit = false;

        public bool active
        {
            get
            {
                return !otherScreenHasFocus && (currentState == screenState.On || currentState == screenState.Active);
            }
        }
        bool otherScreenHasFocus;

        public screenManager SManager
        {
            get 
            { 
                return sManager; 
            }
            internal set 
            { 
                sManager = value; 
            }
        }
        screenManager sManager;

        public PlayerIndex? thisPlayer
        {
            get 
            { 
                return controllingPlayer; 
            }
            internal set 
            { 
                controllingPlayer = value; 
            }
        }
        PlayerIndex? controllingPlayer;

        public virtual void Update(GameTime gameTime, bool otherScreenFocus, bool coveredOtherScreen)
        {
            this.otherScreenHasFocus = otherScreenFocus;

            if (exit)
            {
                currentState = screenState.Off;

                if (!theTransition(gameTime, offT, 1))
                {
                    SManager.RemoveScreen(this);
                }
            }
            else if (coveredOtherScreen)
            {
                if (theTransition(gameTime, offT, 1))
                {
                    currentState = screenState.Off;
                }
                else
                {
                    currentState = screenState.Hidden;
                }
            }
            else
            {
                if (theTransition(gameTime, oT, -1))
                {
                    currentState = screenState.On;
                }
                else
                {
                    currentState = screenState.Active;
                }
            }
        }

        bool theTransition(GameTime gameTime, TimeSpan time, int dir)
        {
            float transitionDelta;

            if (time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
                                          time.TotalMilliseconds);

            pos += transitionDelta * dir;

            if (((dir < 0) && (pos <= 0)) ||
                ((dir > 0) && (pos >= 1)))
            {
                pos = MathHelper.Clamp(pos, 0, 1);
                return false;
            }

            return true;
        }

        public void exitScreen()
        {
            if (offTime == TimeSpan.Zero)
            {
                SManager.RemoveScreen(this);
            }
            else
            {
                exit = true;
            }
        }

        //overrideable methods
        public virtual void LoadContent()
        {

        }
        public virtual void UnloadContent()
        {

        }
        public virtual void HandleInput(inputState input) 
        { 

        }
        public virtual void Draw(GameTime gameTime) 
        {

        }
    }
}
