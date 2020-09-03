using System;

public class FixedJoystick : Joystick
{
    public static FixedJoystick Instance;

    public static bool Touching;

    private void Awake()
    {
        Instance = this;
    }
}