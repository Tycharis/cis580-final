using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CIS580_Final
{
    internal class Button
    {
        /// <summary>
        /// The button texture.
        /// </summary>
        private readonly Texture2D _buttonTexture;

        /// <summary>
        /// The boundaries of the button.
        /// </summary>
        private readonly Rectangle _size;

        /// <summary>
        /// Constructs a new Button instance.
        /// </summary>
        /// <param name="x">Top-left x-coordinate</param>
        /// <param name="y">Top-left y-coordinate</param>
        /// <param name="width">Button width</param>
        /// <param name="height">Button height</param>
        /// <param name="texture">Button texture</param>
        public Button(int x, int y, int width, int height, Texture2D texture)
        {
            _buttonTexture = texture;
            _size = new Rectangle(x, y, width, height);
        }

        /// <summary>
        /// Checks to see if the button has been clicked.
        /// </summary>
        /// <param name="mouseState">The current MouseState object.</param>
        /// <returns></returns>
        public bool IsClicked(MouseState mouseState)
        {
            return mouseState.LeftButton == ButtonState.Pressed && IsMousePositionValid(mouseState);
        }

        /// <summary>
        /// Checks to see if the mouse is within the bounds of the box.
        /// </summary>
        /// <param name="mouseState">The current MouseState object.</param>
        /// <returns></returns>
        private bool IsMousePositionValid(MouseState mouseState)
        {
            int topRightCornerXPos = _size.X + _size.Width; // need reference to the x-coordinate of the top right corner of button
            int bottomLeftCornerYPos = _size.Y + _size.Height; //need reference to the y-coordinate of bottom left corner of button

            bool xCoordIsValid = false;
            bool yCoordIsValid = false;
            
            if(mouseState.Position.X > _size.X && mouseState.Position.X < topRightCornerXPos)
            {
                xCoordIsValid = true;
            }

            if (mouseState.Position.Y > _size.Y && mouseState.Position.Y < bottomLeftCornerYPos)
            {
                yCoordIsValid = true;
            }

            return xCoordIsValid && yCoordIsValid;

        }

        /// <summary>
        /// Draws the Button.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch from the base game class.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_buttonTexture, _size, Color.White);
        }
    }
}
