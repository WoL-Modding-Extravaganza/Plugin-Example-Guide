# Welcome! to modding

tutorial v1. the very basics.

## What you'll need
- Wizard of Legend
- [Visual Studio Community](https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Community&rel=16)
- Any knowledge of c# (or even just willingness to learn)

## Building this mod
This repo is a simple template for you to be able to start modding and build an example mod. 

### Let's do it baybee

1. Download or clone this repo
2. Open the WolModExample.csproj in visual studio
3. Build > Build all 

And we're good!

4. Navigate to `Plugin-Example-Guide/WolModExample/bin/Debug/` and find your `WolModExample.dll`
5. Add that to your BepInEx/plugins folder and you can run the game with the mod
6. Press f1 and f2 to zoom in/out the camera, and if you have a second controller handy, go into coop mode and run around freely

## Alright what's going on
(Under construction)  
Let's take a deeper look at this template so know just what's needed to make a mod. 

### The Project Environment
To be continued.  
This stuff is basically taken care of with the /lib/ folder in our project.  You should be clear to download it and skip to the next section.  
For more information, and/or if you need/want to set it up yourself, see the [bepin docs about setting it up](https://docs.bepinex.dev/articles/dev_guide/plugin_tutorial/1_setup.html). (At the point where you create a project, make it target .net 3.5, and at the point where you're looking for the Assembly-CSharp.dll, use the one from the /lib/ folder in this repo.) 

### BaseUnityPlugin

Also To be continued.  
[bepin docs are pretty good here as well](https://docs.bepinex.dev/articles/dev_guide/plugin_tutorial/2_plugin_start.html)  
You can also look at the `CameraModPlugin.cs` and read the notes about it there.  
To start working on your own mod, you can follow along that link, or you can simply start here in this repo, and delete everything but the `CameraModPlugin` class and the `Awake` function. (little jank I know)  
If you're creating a new project from scratch, add the  `ModAccess.cs` from the repo as well.

## The Code

There are two main ways to write code in a bepin plugin. MMHooks, and HarmonyPatches. For simplicity, I'll just be going over MMHook here. You can read more about Harmony [here](https://github.com/BepInEx/HarmonyX/wiki/Basic-usage)

### MMHook

Hooks are a powerful tool that lets us write code that executes alongside the code of the game.  
To create a hook, it looks something like this:

```c#
On.someclass.somefunction += someclass_somefunction;
```

Start with the `On.` namespace, then go to the class and function you want to hook. Then type `+=` and press the 'tab' key. this will autofill the `someclass_somefunction` part, as well as create a function for the hook to run.

```c#
private void someclass_somefunction(someclass.orig_somefunction orig, someclass self) 
{
	//only nerds allowed past this point 
}
```
This function will have auto-filled parameters for the hook: the original function `orig`, and the original reference `self`.  

`orig` is the original function of the game.  
Somewhere in the code you'll call `orig(self)`.

```c#
private void someclass_somefunction(someclass.orig_somefunction orig, someclass self) 
{
	orig(self);
	//our code here.
}
```
This runs the original code of the game.  
With this you can simply decide to run your code before or after the original code runs.  
Note that if you don't call `orig(self)`, the original function (and any other mods that hook onto it) won't run, and your code will override it.

`self` is the original reference to the class.  
We usually use this to modify local variables of that reference. If you're looking at the class in DNSpy, these are the variables at the bottom.  
In our example, we hooked on to `CameraController.Awake`, so the `self` in that function referred to that particular `CameraController` that shows up in the game.

```c#
private void CameraController_Awake(On.CameraController.orig_Awake orig, CameraController self) 
{
	orig(self);

	self.maxHorizontalDistBetweenPlayers = 100;
	self.maxVerticalDistBetweenPlayers = 100;
	//now we can run really far from each other!
}
```
I believe that should get you started? Go forth and do amazing things!  
Feel free to take a look at the overly commented `CameraModPlugin.cs` and see if you can follow what's going on. If you have any questions go ahead and ask in the Wizard of Legend discord (#modding-extravaganza) and/or ping TheTimesweeper#5727 he craves attention.
