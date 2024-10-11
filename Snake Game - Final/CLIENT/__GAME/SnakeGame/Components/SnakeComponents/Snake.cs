using Client.__GAME.SnakeGame.Components.SnakeComponents;
using Contracts.DataContracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GameComponents
{
    public class Snake
    {
        public Head head;
        public List<SnakeComponent> segments;
        public float speed = 500f;
        public int amountEaten = 0;
        public Color color;
        public int clientID;
        public World world;
        public SnakeSystem snakeSystem;
        public string playerName;
        public bool active = true;
        public float padding = 0.05f;
        public bool invincible = true;

        


        public Snake(int initialSize, Vector2 startingPosition, float initialRadianRotation, World world, int clientID, string playerName, SnakeSystem snakeSystem)
        {
            this.snakeSystem = snakeSystem;
            this.clientID = clientID;
            this.world = world;
            this.playerName = playerName;
            this.color = ColorTable.colors[clientID % 16];

            segments = new List<SnakeComponent>();
            head = new Head(startingPosition, this.world, this);
            head.radianRotation = initialRadianRotation;
            segments.Add(head);

            for (int i = 1; i < initialSize; i++)
            {
                AddSegment();
            }
        }


        public Vector2 CloneVector(Vector2 vector)
        {
            Vector2 newVector = new Vector2(vector.X, vector.Y);
            return newVector;
        }

        public void MoveSnake(GameTime gameTime)
        {
            if (active)
            {
                //RotateTowardsMouse(camera);

                // Calculate the distance the head moves this frame
                float moveDistance = speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Vector2 directionVector = head.GetDirectionVector();

                Vector2 potentialPosition = head.position + directionVector * moveDistance;

                float x = Math.Clamp(potentialPosition.X, world.worldBoundary.upperLeft.X - world.worldBoundary.upperLeft.X * padding, world.worldBoundary.bottomRight.X - world.worldBoundary.bottomRight.X * padding);
                float y = Math.Clamp(potentialPosition.Y, world.worldBoundary.bottomRight.Y - world.worldBoundary.bottomRight.Y * padding, world.worldBoundary.upperLeft.Y - world.worldBoundary.upperLeft.Y * padding);


                Vector2 newHeadPosition = new Vector2(x, y);

                // Update the head's position and its history
                head.UpdatePositionWithHistory(newHeadPosition);

                // Update each segment based on the fifth last position of the segment in front of it
                for (int i = 1; i < segments.Count; i++)
                {
                    if (segments[i - 1].positionHistory.Count == 3)
                    {
                        // We update this segment's position to the fifth last position of the segment ahead
                        Vector2 newPosition = segments[i - 1].positionHistory.Peek();
                        segments[i].UpdatePositionWithHistory(newPosition);
                    }
                }
            }
        }

        public void UpdateSnakePosition(List<Vector2> segmentPositions)
        {
            for(int i = 0; i < segments.Count; i++)
            {
                segments[i].UpdateWorldPosition(segmentPositions[i]);
            }
        }

        public void AddSegment()
        {
            // Reference to the last segment in the snake
            SnakeComponent lastSegment = segments[^1];

            // Create a new body segment that is initially placed at the position of the last segment
            BodySegment newSegment = new BodySegment(lastSegment.position, world, this);

            // If the last segment has a position history, use it to initialize the new segment's history
            if (lastSegment.positionHistory.Count > 0)
            {
                // Copy the last 5 positions from the last segment's history to the new segment's history
                // This assumes the last segment has at least 5 positions in its history; adjust logic as needed
                var historyToCopy = new Queue<Vector2>(lastSegment.positionHistory.Reverse().Take(3));
                foreach (var position in historyToCopy)
                {
                    newSegment.positionHistory.Enqueue(position);
                }
            }
            else
            {
                // If the last segment does not have a history (unlikely, but just in case),
                // initialize the new segment's position history with its current position
                for (int i = 0; i < 3; i++) // Assuming a history size of 5
                {
                    newSegment.positionHistory.Enqueue(newSegment.position);
                }
            }

            // Add the new segment to the snake
            segments.Add(newSegment);
        }

        public void AddSegments(int count)
        {
            for (int i = 0; i < count; i++)
            {
                // Reference to the last segment in the snake
                SnakeComponent lastSegment = segments[^1];

                // Create a new body segment that is initially placed at the position of the last segment
                BodySegment newSegment = new BodySegment(lastSegment.position, world, this);

                // If the last segment has a position history, use it to initialize the new segment's history
                if (lastSegment.positionHistory.Count > 0)
                {
                    // Copy the last 5 positions from the last segment's history to the new segment's history
                    // This assumes the last segment has at least 5 positions in its history; adjust logic as needed
                    var historyToCopy = new Queue<Vector2>(lastSegment.positionHistory.Reverse().Take(3));
                    foreach (var position in historyToCopy)
                    {
                        newSegment.positionHistory.Enqueue(position);
                    }
                }
                else
                {
                    // If the last segment does not have a history (unlikely, but just in case),
                    // initialize the new segment's position history with its current position
                    for (int k = 0; k < 3; k++) // Assuming a history size of 5
                    {
                        newSegment.positionHistory.Enqueue(newSegment.position);
                    }
                }

                // Add the new segment to the snake
                segments.Add(newSegment);
            }
        }

        public void ChangeDirection(float radianRotation)
        {
            head.radianRotation = radianRotation;
        }

        public void Draw(Camera viewportCamera)
        {
            //asdf
            if (active)
            {
                // Draw body segments.
                for (int i = 1; i < segments.Count - 1; i++)
                {
                    segments[i].Draw(viewportCamera);
                }

                // Draw head.
                segments[0].Draw(viewportCamera);

                // Draw Tail segment nad nametag.
                DrawTailAndNametag(viewportCamera);
            }
        }

        private void DrawTailAndNametag(Camera viewportCamera)
        {
            if (!invincible)
            {
                GraphicsManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, viewportCamera.TranslationMatrix);
                GraphicsManager.spriteBatch.Draw(GraphicsManager.tailSegmentTexture, new Vector2(segments[segments.Count - 1].position.X, -segments[segments.Count - 1].position.Y), null, new Color(color.R + 0.45f, color.G + 0.45f, color.B + 0.45f), head.radianRotation, segments[segments.Count - 1].originOffset, segments[segments.Count - 1].renderScaleFactor, SpriteEffects.None, 0);
                GraphicsManager.spriteBatch.DrawString(GraphicsManager.calibriFont, playerName, new Vector2((head.position.X - head.nameLength / 2), -(head.position.Y + 60)), Color.White);
                GraphicsManager.spriteBatch.End();
            }
            else
            {
                GraphicsManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, viewportCamera.TranslationMatrix);
                GraphicsManager.spriteBatch.Draw(GraphicsManager.tailSegmentTexture, new Vector2(segments[segments.Count - 1].position.X, -segments[segments.Count - 1].position.Y), null, new Color(color.R + 0.45f, color.G + 0.45f, color.B + 0.45f, 0.25f), head.radianRotation, segments[segments.Count - 1].originOffset, segments[segments.Count - 1].renderScaleFactor, SpriteEffects.None, 0);
                GraphicsManager.spriteBatch.DrawString(GraphicsManager.calibriFont, playerName, new Vector2((head.position.X - head.nameLength / 2), -(head.position.Y + 60)), Color.White);
                GraphicsManager.spriteBatch.End();
            }
        }

        public void MakeChangeDirectionRequest(float radianRotation, Input input)
        {
            head.radianRotation = radianRotation;

            DirectionChangeRequestData directionChangeRequestData = new DirectionChangeRequestData(clientID, radianRotation);
            input.directionChangeRequestDatas.Add(directionChangeRequestData);
        }

        public void PositionSnake(Vector2 position)
        {
            Vector2 newHeadPosition = new Vector2(position.X, position.Y);

            // Update the head's position and its history
            head.UpdatePositionWithHistory(newHeadPosition);

            // Update each segment based on the fifth last position of the segment in front of it
            for (int i = 1; i < segments.Count; i++)
            {
                if (segments[i - 1].positionHistory.Count == 3)
                {
                    // We update this segment's position to the fifth last position of the segment ahead
                    Vector2 newPosition = segments[i - 1].positionHistory.Peek();
                    segments[i].UpdatePositionWithHistory(newPosition);
                }
            }
        }

        public void RemoveAllSegmentsButHead()
        {
            int segmentsToRemove = segments.Count - 1;

            for (int i = 0; i < segmentsToRemove; i++)
            {
                segments[segments.Count - 1].DeleteSegment();
                segments.RemoveAt(segments.Count - 1);
            }
        }


        private void RotateTowardsMouse(Camera camera)
        {
            // Get the current mouse state and translate its position to world coordinates
            Vector2 mousePosition = Mouse.GetState().Position.ToVector2();
            Vector2 mouseWorldPosition = camera.ScreenToWorld(mousePosition);

            // Calculate the direction vector from the head's position to the mouse's position in the world
            Vector2 direction = mouseWorldPosition - new Vector2(head.position.X, head.position.Y);

            // Calculate the rotation angle from the direction vector
            head.radianRotation = (float)Math.Atan2(direction.Y, direction.X);
        }
    }
}
