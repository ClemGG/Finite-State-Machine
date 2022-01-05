# Finite-State-Machine
A Generic Finite State Machine System made in Unity 2021.
This asset allows you to implement a generic state machine to implement a behaviour hierarchy for your projects (controllers, UI panels, etc.)
This system is very flexible and classes can be overidden to suit your needs. It is recommended to use the custom script template instead of creating new states from scratch (Right Click > Create > Custom Script Templates > State Machine > New State Script). This will allow your script to inherit all necessary methods for ease of use.

This system implements Object Pooling to recycle used states instead of recreating them each time in order to avoid unecessary garbage collection and memory issues. The Object Pooling system uses generic methods as well to allow you to store any kind of classes (including custom classes, MonoBehaviours and ScriptableObjects) by specifying a size and a creation function in the StateMachienFactory's constructor.

The demo scene implements 3 states : 2 'root states' which can be swapped between to change the appearance of the 'State machine Tester' object, and 1 'sub-state' used to implement extra behaviour for one of the main states. The StateMachineFactory class responsible for managing the states uses 3 parameters in its constructor : 
- A 'context' object, which is the object holding all the variables and Components to modify;
- An 'input' object,  which allows dynamic interaction with the user (useful in an online mode where a state needs to access the output of a specific user). This project uses the new InputSystem for its demo's input class;
- A list of 'pools' used by the Object Pooler to create and retrieve the different states (see the StateMachine tester class for a concise implementation).
