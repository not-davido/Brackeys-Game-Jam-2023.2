using UnityEngine;

public static class Events {
    public static PlayerDeathEvent PlayerDeathEvent = new();
    public static GamePauseEvent GamePauseEvent = new();
    public static GameQuitEvent GameQuitEvent = new();
    public static GameCompletedEvent GameCompletedEvent = new();
    public static LevelTransitionEvent LevelTransitionEvent = new();
    public static CollectiblePickUpEvent CollectiblePickUpEvent = new();
    public static PlayerWinEvent PlayerWinEvent = new();
}

public class GameEvent { }

public class PlayerDeathEvent : GameEvent { }

public class PlayerDamageEvent : GameEvent { }

public class GamePauseEvent : GameEvent {
    public bool paused;
}

public class GameQuitEvent : GameEvent { }

public class GameCompletedEvent : GameEvent { }

public class LevelTransitionEvent : GameEvent {
    public bool isTransitioningIn;
    public bool isTransitioningOut;
    /// <summary>
    /// An optional new position after transitioning.
    /// </summary>
    public Vector2 newPosition;
}

public class CollectiblePickUpEvent : GameEvent { }

public class PlayerWinEvent : GameEvent { }