using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

#if WINDOWS_PHONE
using Microsoft.Devices.Sensors;
#endif

namespace MyPlatformGame
{
  public class AccelerometerState
  {
    public Vector3 Acceleration = Vector3.Zero;

    public AccelerometerState()
    {
    }
  }
#if  WINDOWS || XBOX
    public class Accelerometer
  {
    public static void Update()
    {
      var gp = GamePad.GetState(PlayerIndex.One);
      state.Acceleration.X = gp.ThumbSticks.Left.X;
      state.Acceleration.Y = gp.ThumbSticks.Left.Y;

      //state.Acceleration.Z = gp.Triggers.Left;
    }

    static AccelerometerState state = new AccelerometerState();

    public static AccelerometerState GetState()
    {
      return state;
    }
  }
#endif

#if WINDOWS_PHONE
  public class Accelerometer
  {
    private static Microsoft.Devices.Sensors.Accelerometer acceleroSensor = new Microsoft.Devices.Sensors.Accelerometer();

    static Accelerometer()
    {
      acceleroSensor.ReadingChanged += new EventHandler<AccelerometerReadingEventArgs>(Default_ReadingChanged);
      acceleroSensor.Start();
    }

    static AccelerometerState state = new AccelerometerState();

    static void Default_ReadingChanged(object sender,
                                      AccelerometerReadingEventArgs e)
    {
      state.Acceleration.X = (float)e.X;
      state.Acceleration.Y = (float)e.Y;
      state.Acceleration.Z = (float)e.Z;
    }

    public static AccelerometerState GetState()
    {
      return state;
    }
  }
#endif
}
