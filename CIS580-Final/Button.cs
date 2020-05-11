using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CIS580_Final
{
    public class Button
    {
        private int PositionX;//x coordinate at the top-left of the button
        private int PositionY; //y coordinate at the top left of the button

        private int ButtonWidth; //width of the button
        private int ButtonHeight; //height of the button NOTE :Height goes downwards in monogame.
        

        Game1 game;
        Texture2D buttonTexture;
        Rectangle size;
        
        
        
        public Button(int SetX, int SetY, int SetWidth, int SetHeight, Texture2D SetTexture)
        {
            SetPosition(SetX, SetY);
            SetSize(SetWidth, SetHeight);
            buttonTexture = SetTexture;
            size = new Rectangle(SetX, SetY, SetWidth, SetHeight);
            
        }

        public void SetPosition(int x, int y)
        {
            PositionX = x;
            PositionY = y;
        }

        public void SetSize(int width, int height)
        {
            ButtonWidth = width;
            ButtonHeight = height;
        }

        public bool IsClicked(MouseState mouseState)
        {
            if(mouseState.LeftButton == ButtonState.Pressed && (IsMousePositionValid(mouseState) == true) )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsMousePositionValid(MouseState mouseState)
        {
            int TopRightCornerXPos = PositionX + ButtonWidth; // need reference to the X coord of the top right corner of button
            int bottomLeftCornerYPos = PositionY + ButtonHeight; //need reference to the Y coord of bottom left corner of button

            bool XCoordIsValid = false;
            bool YCoordIsValid = false;
            
            if(mouseState.Position.X > PositionX && mouseState.Position.X < TopRightCornerXPos)
            {
                XCoordIsValid = true;
            }
            if (mouseState.Position.Y > PositionY && mouseState.Position.Y < bottomLeftCornerYPos)
            {
                YCoordIsValid = true;
            }

            if (XCoordIsValid == true && YCoordIsValid == true)
            {
                return true;
            }
            else
            {
                return false;
            }

        }// end IsMousePosition

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(buttonTexture, size, Color.White);


        }



    }//end class
}//end namespace
