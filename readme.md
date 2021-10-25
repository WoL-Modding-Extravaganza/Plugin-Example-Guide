# Welcome! to modding

Guide v1. the very basics.

## What you'll need
- Wizard of Legend
- [Visual Studio Community](https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Community&rel=16)
- Any knowledge of c# (or even just willingness to learn)

## Building this mod
This repo is a simple template for you to be able to start modding and build an example mod. 

### Let's do it baybee

1. Download or clone this repo anywhere on your computer.
2. Open the WolModExample.csproj in visual studio (now's the time to download it if you don't have it)
3. At the top, go to `Build > Build WolModExample`

And we're good! The you have just built the mod. You can now add the mod to your game:

4. Navigate to `Plugin-Example-Guide/WolModExample/bin/Debug/` and find your `WolModExample.dll`
5. Add that to your `BepInEx/plugins` folder and you can run the game with the mod
6. Press f1 and f2 to zoom in/out the camera, and if you have a second controller handy, go into coop mode and run around freely

## Alright what's going on
Let's take a deeper look at this template so know just what's needed to make a mod.

(Under construction)  
In all transparency, the modding scene for this game is not in a state where you can find a step-by-step guide on creating a mod.  
However, the resources are there if you want to learn. It's in a state where it is definitely possible to figure out if you dive in to it. Follow this project as best as you can, and ask questions in the modding discord.  

### The Project Environment
(Under Construction)  
If you've downloaded this repo, you should be clear to open the project and skip to the next section. it's a working project environment you can use to create a mod.  
To make your own project from scratch, quick and dirty guide is:  
- open Visual Studio Community
- create a new Class Library (c#)
- grab the `/lib/` folder from this repo and place it in your project directory
- in the project, [add reference](https://docs.microsoft.com/en-us/visualstudio/ide/managing-references-in-a-project?view=vs-2019) to all the dlls in the `/lib/` folder
- as well, make sure you grab the [ModAccess.cs](https://github.com/WoL-Modding-Extravaganza/Plugin-Example-Guide/blob/main/WolModExample/ModAccess.cs) script and place that in your project.
- take a look at [CameraModPlugin.cs](https://github.com/WoL-Modding-Extravaganza/Plugin-Example-Guide/blob/main/WolModExample/CameraModPlugin.cs), and use that as a basis in your new c# class. Main three things to copy over are the `BepInPlugin` attribute in brackets, the class `public class myclass : BaseUnityPlugin`, and the function `void Awake` within it.

### BaseUnityPlugin

(Also Under Construction)  
Take a look at the [CameraModPlugin.cs](https://github.com/WoL-Modding-Extravaganza/Plugin-Example-Guide/blob/main/WolModExample/CameraModPlugin.cs) file and at the top, read the `BepInPlugin Notes` and the `BaseUnityPlugin Notes`.  

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

Take a look at the overly commented `CameraModPlugin.cs` and see if you can follow what's going on.  
Absolutely get [dnSpy](https://github.com/risk-of-thunder/R2Wiki/wiki/Code-Analysis-with-dnSpy) if you haven't, so you can [look at the game's code](https://github.com/risk-of-thunder/R2Wiki/wiki/Code-Analysis-with-dnSpy). (link stolen from ror2 modding wiki (but applies here of course))  

If you have any questions go ahead and ask in the [Wizard of Legend discord](https://discord.gg/wizardoflegend) (#modding-extravaganza) and/or ping TheTimesweeper#5727 he craves attention.

Also of course give any and all feedback on this guide will be very appreciated. Thanks thanks have a lovely evening c:
