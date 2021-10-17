# TinyECA
### TinyECA - Entity Component Architecture

TinyECA is a tiny project that contains the basics of a Unity-like entity component architecture.

It's extremely simple to use which makes it perfect for small to mid-size applications.

Performance Note: This project is not supposed to be super performant, it's main use is to be a quick implementation of a Unity-like Entity Component Architecture.

## Basic Usage
All the classes are contained in the TinyECA namespace.


### EntityScene
An EntityScene is simply a container for entities that  makes it easy to only update the entities you want.

Creating an EntityScene is as simple as instantiating a new EntityScene.
```cs
EntityScene scene = new EntityScene();
```

### Entities
Once an EntityScene has been created, you can start populating it with entities.

To do so, all you need to do is to call the CreateEntity method of a scene with a name. If no name is provided the default name will be "New Entity"

```cs
Entity entity = scene.CreateEntity("My Entity");
```

names are simply used to search for entities in a scene, Although they can easily be expanded upon since the name property is public.

To remove an entity from a scene, call the RemoveEntity method of a scene

```cs
scene.RemoveEntity(entity);
```

Finding entities with a name is as simple as using the scene.FindEntitiesWithName method of the scene and providing it with the name

```cs
Entity[] entities = scene.FindEntitiesWithName("Some Name");
```

### EntityComponent

Last but not least, there's the component. The EntityComponent is a very simple class that contains both data and methods that can interact with other components.

Creating a component is as easy as deriving a class from EntityComponent.

```cs
class MyComponent : EntityComponent
{
	private void OnCreate()
   	{
    	Console.WriteLine("My component was created");
    }
}
```

The OnCreate method of a component is a special function that gets called when a component gets added to an entity. There is also a matching OnDestroy that gets called when a component is removed from an entity.

To add this component to an entity, call the AddComponent method of an entity and give it the component type.

```cs
entity.AddComponent<MyComponent>();
```

AddComponent returns the instance of the component that was added.

To remove a component from an entity, call the corrisponding RemoveComponent method of an entity and give it the component type.

```cs
entity.RemoveComponent<MyComponent>();
```

And lastly to get a component from an entity, call the GetComponent method.

```cs
MyComponent component = entity.GetComponent<MyComponent>();
```

### Component Methods and Properties

Components have a few built-in methods like AddComponent, GetComponent and RemoveComponent that operate on the entity the component is on.

Components can also access their name, and parent entity.

```cs
class MyComponent : EntityComponent
{
	private void OnCreate()
   	{
    	string myName = Name; // 'Name' is the name of the entity
    	Console.WriteLine($"Hello, My name is {myName}");
    }
    
    public void DoStuff()
    {
    	// Do Stuff...
    }
}

class MyComponentOther : EntityComponent
{
	private void OnCreate()
   	{
        AddComponent<MyComponent>(); // This also returns an instance
        
        MyComponent component = GetComponent<MyComponent>();
        component.DoStuff();
        
        RemoveComponent();
        
        // 'Parent' is the parent entity the component is attached to 
        // and 'Scene' is of course the scene that the entity is in 
        Parent.Scene.RemoveEntity(Parent); // This removes the parent entity from the scene
    }
}
```

### Calling common component methods

Now for the interesting part. Calling methods by name on all components in a scene.

This is very useful for updating components and giving them additional functionality.

There are a few methods that do this:

```cs
class FirstComponent : EntityComponent
{
	private void CustomMethod()
    {
		Console.WriteLine("This is my custom function");
	}
}

class SecondComponent : CustomMethod
{
	private void CustomFunction()
    {
		Console.WriteLine("This is my custom function");
	}
}
```
The first is the CallComponentMethod in the scene. This will call every method with that name in every component in the scene.
```cs
EntityScene scene = new EntityScene();
Entity entity1 = scene.CreateEntity();
Entity entity2 = scene.CreateEntity();

entity1.AddComponent<FirstComponent>();
entity2.AddComponent<FirstComponent>();
scene.CallComponentMethod("CustomMethod");
```
The second is the CallComponentMethod in the entity. This will call every method with that name in every component that's attached to that entity.
```cs
EntityScene scene = new EntityScene();
Entity entity = scene.CreateEntity();

entity.AddComponent<FirstComponent>();
entity.AddComponent<SecondComponent>();
entity.CallComponentMethod("CustomMethod");
```
The third and final is the CallComponentMethod<> in both the scene and entity that allows you to narrow down the method calls to a specific component
```cs
EntityScene scene = new EntityScene();
Entity entity = scene.CreateEntity();

entity.AddComponent<FirstComponent>();
entity.AddComponent<SecondComponent>();
scene.CallComponentMethod<MyComponent>("CustomMethod");
```

The methods can also take in parameters in the form of an object array.

Parameter mismatch will result in an exception.

```cs
class MyComponent : EntityComponent
{
	private void CustomMethod(string text)
    {
		Console.WriteLine(text);
	}
}
```
```cs
EntityScene scene = new EntityScene();
Entity entity = scene.CreateEntity();

entity.AddComponent<MyComponent>();
scene.CallComponentMethod<MyComponent>("CustomMethod", new object[] { "Hello, World" });
```


## Example Project

```cs
using System;
using TinyECA;

namespace TestProject
{
    // Counter component to keep the sum of all
    // the values passed in with the Add method
    class Counter : EntityComponent
    {
        // Keep track of the count
        private int count = 0;
        
        // Add method that's called on all
        // components in the main program
        private void Add(int amount)
        {
            // Increment with the amount provided
            count += amount;
        }

        // Getter for the display component
        public int GetCount() => count;
    }
    
    // Component that displays the final count
    class CounterDisplay : EntityComponent
    {
        // Display method that's called on all
        // CounterDisplay components in the main program
        private void Display()
        {
            // Get the counter
            Counter counter = GetComponent<Counter>();
            if (counter != null) // If it exists, display the count
                Console.WriteLine(counter.GetCount());
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            // Create the scene
            EntityScene scene = new EntityScene();
            
            // Create the first counter
            Entity counter1 = scene.CreateEntity("Player");
            counter1.AddComponent<Counter>();
            counter1.AddComponent<CounterDisplay>();
            
            // Add 2 to all counters
            scene.CallComponentMethod("Add", new object[] { 2 });

            // Create the second counter
            Entity counter2 = scene.CreateEntity("Player");
            counter2.AddComponent<Counter>();
            counter2.AddComponent<CounterDisplay>();
            
            // Add 5 to all counters
            scene.CallComponentMethod("Add", new object[] { 5 });
            
            // Display the values of all the counters
            scene.CallComponentMethod<CounterDisplay>("Display");
            
            // Result is 7 and 5
        }
    }
}
```
