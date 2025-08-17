using UnityEngine;

public class Task
{
    public string Description;
    

    public TaskType Type;
    
}

public enum TaskType
{
    CleanOffice,
    SendSupplies,
    KeepDoorOpen,
    KeepLightsOn,
}

