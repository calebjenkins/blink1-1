namespace Sleddog.Blink1.Internal
{
    internal enum Blink1Commands : byte
    {
        Test = (byte) '!',
        GetVersion = (byte) 'v',
        SetColor = (byte) 'n',
        FadeToColor = (byte) 'c',
        InactivityMode = (byte) 'D',
        PresetControl = (byte) 'p',
        ReadPreset = (byte) 'R',
        SavePreset = (byte) 'P',
        SavePresetMk2 = (byte) 'W',
        ReadPlaybackState = (byte) 'S',
    }
}