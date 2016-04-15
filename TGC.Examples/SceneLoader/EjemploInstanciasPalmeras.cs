using Microsoft.DirectX;
using System.Collections.Generic;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Geometries;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;
using TGC.Util;

namespace TGC.Examples.SceneLoader
{
    /// <summary>
    ///     Ejemplo EjemploInstanciasPalmeras
    ///     Unidades Involucradas:
    ///     # Unidad 3 - Conceptos B�sicos de 3D - Mesh
    ///     # Unidad 7 - T�cnicas de Optimizaci�n - Instancias de Modelos
    ///     Muestra como crear varias instancias de un mismo TgcMesh.
    ///     De esta forma se reutiliza su informaci�n gr�fica (tri�ngulos, v�rtices, textura, etc).
    ///     Autor: Mat�as Leone, Leandro Barbagallo
    /// </summary>
    public class EjemploInstanciasPalmeras : TgcExample
    {
        private List<TgcMesh> meshes;
        private TgcMesh palmeraOriginal;
        private TgcBox suelo;

        public override string getCategory()
        {
            return "SceneLoader";
        }

        public override string getName()
        {
            return "Instancias Palmeras";
        }

        public override string getDescription()
        {
            return "Muestra como crear varias instancias de un mismo TgcMesh.";
        }

        public override void init()
        {
            //Crear suelo
            var pisoTexture = TgcTexture.createTexture(D3DDevice.Instance.Device,
                GuiController.Instance.ExamplesMediaDir + "Texturas\\pasto.jpg");
            suelo = TgcBox.fromSize(new Vector3(500, 0, 500), new Vector3(2000, 0, 2000), pisoTexture);

            //Cargar modelo de palmera original
            var loader = new TgcSceneLoader();
            var scene =
                loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir +
                                         "MeshCreator\\Meshes\\Vegetacion\\Palmera\\Palmera-TgcScene.xml");
            palmeraOriginal = scene.Meshes[0];

            //Crear varias instancias del modelo original, pero sin volver a cargar el modelo entero cada vez
            var rows = 5;
            var cols = 6;
            float offset = 200;
            meshes = new List<TgcMesh>();
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    //Crear instancia de modelo
                    var instance = palmeraOriginal.createMeshInstance(palmeraOriginal.Name + i + "_" + j);

                    //Desplazarlo
                    instance.move(i * offset, 70, j * offset);
                    instance.Scale = new Vector3(0.25f, 0.25f, 0.25f);

                    meshes.Add(instance);
                }
            }

            //Camara en primera persona
            GuiController.Instance.FpsCamera.Enable = true;
            GuiController.Instance.FpsCamera.MovementSpeed = 400;
            GuiController.Instance.FpsCamera.JumpSpeed = 400;
            GuiController.Instance.FpsCamera.setCamera(new Vector3(61.8657f, 403.7024f, -527.558f),
                new Vector3(379.7143f, 12.9713f, 336.3295f));
        }

        public override void render(float elapsedTime)
        {
            //Renderizar suelo
            suelo.render();

            //Renderizar instancias
            foreach (var mesh in meshes)
            {
                mesh.render();
            }
        }

        public override void close()
        {
            suelo.dispose();

            //Al hacer dispose del original, se hace dispose autom�ticamente de todas las instancias
            palmeraOriginal.dispose();
        }
    }
}