using Aura3D.Avalonia;
using Aura3D.Core;
using Aura3D.Core.Nodes;
using Avalonia.Platform;

namespace NewBeeUI.Demo.Views;

public class Glb3DView : BaseView
{
    protected override object Build()
    {
        var v = new Aura3DView()
        {
            Width = 300,
            Height = 300,
        };
        v.SceneInitialized += (sender, e) =>
        {
            var view = sender as Aura3DView;

            if (view == null) return;

            // Version 0.0.1 requires manual instantiation of the camera
            var camera = new Camera();
            // For other versions or newer releases, the main camera is built-in
            //var camera = view.MainCamera;

            camera.ClearColor = System.Drawing.Color.White;

            view.AddNode(camera);

            var glbStream = AssetLoader.Open(new Uri($"avares://NewBeeUI.Demo/Assets/ainimal.glb"));

            var model = ModelLoader.LoadGlbModel(glbStream);

            model.Position = camera.Forward * 3;

            view.AddNode(model);

        };
        return v;
    }
}
