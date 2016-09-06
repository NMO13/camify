using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using Shared.Geometry;

namespace Shared.Export
{
    public class MeshExporter
    {
        static NumberFormatInfo nfi = new NumberFormatInfo();

        public static void GenerateCollada(List<Mesh> meshes, String path)
        {
            nfi.NumberDecimalSeparator = ".";
            using (FileStream fileStream = new FileStream(Path.Combine(path, Path.GetRandomFileName() + ".dae"), FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fileStream))
            using (XmlTextWriter writer = new XmlTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 2;

                writer.WriteStartDocument();
                writer.WriteStartElement("COLLADA", "http://www.collada.org/2005/11/COLLADASchema");
                writer.WriteAttributeString("version", "1.4.1");

                SetAsset(writer);
                SetLibraryCameras(writer);
                SetLibraryLights(writer);
                SetLibraryEffects(writer);
                SetLibraryMaterials(writer);
                SetLibraryGeometries(writer, meshes);
                SetLibraryVisualScenes(writer, meshes);

                writer.WriteStartElement("scene");
                writer.WriteStartElement("instance_visual_scene");
                writer.WriteAttributeString("url", "#Scene");
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteEndElement(); //COLLADA
                writer.WriteEndDocument();
            }
        }


        private static void SetAsset(XmlWriter writer)
        {
            writer.WriteStartElement("asset");

            writer.WriteStartElement("contributor");
            writer.WriteElementString("author", "Blender User");
            writer.WriteElementString("authoring_tool", "Blender 2.74.0 commit date:2015-03-31, commit time:13:39, hash:000dfc0");
            writer.WriteEndElement();

            writer.WriteElementString("created", "2016-02-01T23:27:06");
            writer.WriteElementString("modified", "2016-02-01T23:27:06");

            writer.WriteStartElement("unit");
            writer.WriteAttributeString("name", "meter");
            writer.WriteAttributeString("meter", "1");
            writer.WriteEndElement();

            writer.WriteElementString("up_axis", "Z_UP");

            writer.WriteEndElement();
        }

        private static void SetLibraryCameras(XmlTextWriter writer)
        {
            writer.WriteStartElement("library_cameras");

            writer.WriteStartElement("camera");
            writer.WriteAttributeString("id", "Camera-camera");
            writer.WriteAttributeString("name", "Camera");

            writer.WriteStartElement("optics");
            writer.WriteStartElement("technique_common");
            writer.WriteStartElement("perspective");

            writer.WriteStartElement("xfov");
            writer.WriteAttributeString("sid", "xfov");
            writer.WriteString("49.13434");
            writer.WriteEndElement();

            writer.WriteElementString("aspect_ratio", "1.777778");

            writer.WriteStartElement("znear");
            writer.WriteAttributeString("sid", "znear");
            writer.WriteString("0.1");
            writer.WriteEndElement();

            writer.WriteStartElement("zfar");
            writer.WriteAttributeString("sid", "zfar");
            writer.WriteString("100");
            writer.WriteEndElement();

            writer.WriteEndElement(); //perspective
            writer.WriteEndElement(); //technique_common
            writer.WriteEndElement(); //optics

            writer.WriteStartElement("extra");
            writer.WriteStartElement("technique");
            writer.WriteAttributeString("profile", "blender");

            writer.WriteElementString("YF_dofdist", "0");
            writer.WriteElementString("shiftx", "0");
            writer.WriteElementString("shifty", "0");

            writer.WriteEndElement(); //technique

            writer.WriteEndElement(); //extra

            writer.WriteEndElement(); //camera
            writer.WriteEndElement(); //library_cameras
        }

        private static void SetLibraryLights(XmlTextWriter writer)
        {
            writer.WriteStartElement("library_lights");
            writer.WriteStartElement("light");
            writer.WriteAttributeString("id", "Lamp-light");
            writer.WriteAttributeString("name", "Lamp");

            writer.WriteStartElement("technique_common");
            writer.WriteStartElement("point");

            writer.WriteStartElement("color");
            writer.WriteAttributeString("sid", "color");
            writer.WriteString("1 1 1");
            writer.WriteEndElement();

            writer.WriteElementString("constant_attenuation", "1");
            writer.WriteElementString("linear_attenuation", "0");
            writer.WriteElementString("quadratic_attenuation", "0.00111109");

            writer.WriteEndElement(); //point
            writer.WriteEndElement(); //technique_commen

            writer.WriteStartElement("extra");
            writer.WriteStartElement("technique");
            writer.WriteAttributeString("profile", "blender");

            writer.WriteElementString("adapt_thresh", "0.000999987");
            writer.WriteElementString("area_shape", "1");
            writer.WriteElementString("area_size", "0.1");
            writer.WriteElementString("area_sizey", "0.1");
            writer.WriteElementString("area_sizez", "1");
            writer.WriteElementString("atm_distance_factor", "1");
            writer.WriteElementString("atm_extinction_factor", "1");
            writer.WriteElementString("atm_turbidity", "2");
            writer.WriteElementString("att1", "0");
            writer.WriteElementString("att2", "1");
            writer.WriteElementString("backscattered_light", "1");
            writer.WriteElementString("bias", "1");
            writer.WriteElementString("blue", "1");
            writer.WriteElementString("buffers", "1");
            writer.WriteElementString("bufflag", "0");
            writer.WriteElementString("bufsize", "2880");
            writer.WriteElementString("buftype", "2");
            writer.WriteElementString("clipend", "30.002");
            writer.WriteElementString("clipsta", "1.000799");
            writer.WriteElementString("compressthresh", "0.04999995");

            writer.WriteStartElement("dist");
            writer.WriteAttributeString("sid", "blender_dist");
            writer.WriteString("29.99998");
            writer.WriteEndElement();

            writer.WriteStartElement("energy");
            writer.WriteAttributeString("sid", "blender_energy");
            writer.WriteString("1");
            writer.WriteEndElement();

            writer.WriteElementString("falloff_type", "2");
            writer.WriteElementString("filtertype", "0");
            writer.WriteElementString("flag", "0");

            writer.WriteStartElement("gamma");
            writer.WriteAttributeString("sid", "blender_gamma");
            writer.WriteString("1");
            writer.WriteEndElement();

            writer.WriteElementString("green", "1");

            writer.WriteStartElement("halo_intensity");
            writer.WriteAttributeString("sid", "blnder_halo_intensity");
            writer.WriteString("1");
            writer.WriteEndElement();

            writer.WriteElementString("horizon_brightness", "1");
            writer.WriteElementString("mode", "8192");
            writer.WriteElementString("ray_samp", "1");
            writer.WriteElementString("ray_samp_method", "1");
            writer.WriteElementString("ray_samp_type", "0");
            writer.WriteElementString("ray_sampy", "1");
            writer.WriteElementString("ray_sampz", "1");
            writer.WriteElementString("red", "1");
            writer.WriteElementString("samp", "3");
            writer.WriteElementString("shadhalostep", "0");

            writer.WriteStartElement("shadow_b");
            writer.WriteAttributeString("sid", "blender_shadow_b");
            writer.WriteString("0");
            writer.WriteEndElement();

            writer.WriteStartElement("shadow_g");
            writer.WriteAttributeString("sid", "blender_shadow_g");
            writer.WriteString("0");
            writer.WriteEndElement();

            writer.WriteStartElement("shadow_r");
            writer.WriteAttributeString("sid", "blender_shadow_r");
            writer.WriteString("0");
            writer.WriteEndElement();

            writer.WriteElementString("sky_colorspace", "0");
            writer.WriteElementString("sky_exposure", "1");
            writer.WriteElementString("skyblendfac", "1");
            writer.WriteElementString("skyblendtype", "1");
            writer.WriteElementString("soft", "3");
            writer.WriteElementString("spotblend", "0.15");
            writer.WriteElementString("spotsize", "75");
            writer.WriteElementString("spread", "1");
            writer.WriteElementString("sun_brightness", "1");
            writer.WriteElementString("sun_effect_type", "0");
            writer.WriteElementString("sun_intensity", "1");
            writer.WriteElementString("sun_size", "1");
            writer.WriteElementString("type", "0");

            writer.WriteEndElement(); //technique
            writer.WriteEndElement(); //extra

            writer.WriteEndElement(); //light
            writer.WriteEndElement(); //library_lights

            writer.WriteStartElement("library_images");
            writer.WriteEndElement();
        }

        private static void SetLibraryEffects(XmlTextWriter writer)
        {
            writer.WriteStartElement("library_effects");
            writer.WriteStartElement("effect");
            writer.WriteAttributeString("id", "Material-effect");

            writer.WriteStartElement("profile_COMMON");
            writer.WriteStartElement("technique");
            writer.WriteAttributeString("sid", "common");

            writer.WriteStartElement("phong");

            writer.WriteStartElement("emission");
            writer.WriteStartElement("color");
            writer.WriteAttributeString("sid", "emission");
            writer.WriteString("0 0 0 1");
            writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteStartElement("ambient");
            writer.WriteStartElement("color");
            writer.WriteAttributeString("sid", "ambient");
            writer.WriteString("0 0 0 1");
            writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteStartElement("diffuse");
            writer.WriteStartElement("color");
            writer.WriteAttributeString("sid", "diffuse");
            writer.WriteString("0.64 0.64 0.64 1");
            writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteStartElement("specular");
            writer.WriteStartElement("color");
            writer.WriteAttributeString("sid", "specular");
            writer.WriteString("0.5 0.5 0.5 1");
            writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteStartElement("shininess");
            writer.WriteStartElement("float");
            writer.WriteAttributeString("sid", "shininess");
            writer.WriteString("50");
            writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteStartElement("index_of_refraction");
            writer.WriteStartElement("float");
            writer.WriteAttributeString("sid", "index_of_refraction");
            writer.WriteString("1");
            writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteEndElement(); //phong
            writer.WriteEndElement(); //technique

            writer.WriteEndElement(); //profile_COMMON
            writer.WriteEndElement(); //effect
            writer.WriteEndElement(); //library_effects
        }

        private static void SetLibraryMaterials(XmlTextWriter writer)
        {
            writer.WriteStartElement("library_materials");

            writer.WriteStartElement("material");
            writer.WriteAttributeString("id", "Material-material");
            writer.WriteAttributeString("name", "Material");

            writer.WriteStartElement("instance_effect");
            writer.WriteAttributeString("url", "#Material-effect");
            writer.WriteEndElement();

            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        private static void SetLibraryGeometries(XmlTextWriter writer, List<Mesh> meshes)
        {
            writer.WriteStartElement("library_geometries");

            for (int i = 0; i < meshes.Count; i++)
            {
                var objectID = "Object" + i;

                writer.WriteStartElement("geometry");
                writer.WriteAttributeString("id", objectID + "-mesh");
                writer.WriteAttributeString("name", objectID);

                writer.WriteStartElement("mesh");

                SetPositions(writer, meshes[i], objectID);
                SetNormals(writer, meshes[i], objectID);

                writer.WriteStartElement("vertices");
                writer.WriteAttributeString("id", objectID + "-mesh-vertices");

                writer.WriteStartElement("input");
                writer.WriteAttributeString("semantic", "POSITION");
                writer.WriteAttributeString("source", "#" + objectID + "-mesh-positions");

                writer.WriteEndElement(); //input
                writer.WriteEndElement(); //vertices

                SetPolyList(writer, meshes[i], objectID);

                writer.WriteEndElement(); //mesh
                writer.WriteEndElement(); //geometry
            }

            writer.WriteEndElement(); //library_geometries

            writer.WriteStartElement("library_controllers");
            writer.WriteEndElement();
        }

        private static void SetPositions(XmlTextWriter writer, Mesh mesh, string objectId)
        {
            writer.WriteStartElement("source");
            writer.WriteAttributeString("id", objectId + "-mesh-positions");

            writer.WriteStartElement("float_array");
            writer.WriteAttributeString("id", objectId + "-mesh-positions-array");
            writer.WriteAttributeString("count", (mesh.Vertices.Length * 3).ToString(nfi));
            foreach (var vertex in mesh.Vertices)
            {
                writer.WriteString(vertex.X.ToString(nfi) + " ");
                writer.WriteString(vertex.Y.ToString(nfi) + " ");
                writer.WriteString(vertex.Z.ToString(nfi) + " ");
            }
            writer.WriteEndElement(); //float_array

            writer.WriteStartElement("technique_common");
            writer.WriteStartElement("accessor");
            writer.WriteAttributeString("source", "#" + objectId + "-mesh-positions-array");
            writer.WriteAttributeString("count", (mesh.Vertices.Length.ToString(nfi)));
            writer.WriteAttributeString("stride", "3");

            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "X");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement();

            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "Y");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement();

            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "Z");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement();

            writer.WriteEndElement(); //technique_common
            writer.WriteEndElement(); //accessor
            writer.WriteEndElement(); //source
        }

        private static void SetNormals(XmlTextWriter writer, Mesh mesh, string objectId)
        {
            writer.WriteStartElement("source");
            writer.WriteAttributeString("id", objectId + "-mesh-normals");

            writer.WriteStartElement("float_array");
            writer.WriteAttributeString("id", objectId + "-mesh-normals-array");
            writer.WriteAttributeString("count", ((mesh.RenderNormals.Length * 3).ToString(nfi)));
            foreach (var normal in mesh.RenderNormals)
            {
                writer.WriteString(normal.X.ToString(nfi) + " ");
                writer.WriteString(normal.Y.ToString(nfi) + " ");
                writer.WriteString(normal.Z.ToString(nfi) + " ");
            }
            writer.WriteEndElement(); //float_array

            writer.WriteStartElement("technique_common");
            writer.WriteStartElement("accessor");
            writer.WriteAttributeString("source", "#" + objectId + "-mesh-normals-array");
            writer.WriteAttributeString("count", mesh.RenderNormals.Length.ToString(nfi));
            writer.WriteAttributeString("stride", "3");

            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "X");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement();

            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "Y");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement();

            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "Z");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement();

            writer.WriteEndElement(); //technique_common
            writer.WriteEndElement(); //accessor

            writer.WriteEndElement(); //source
        }

        private static void SetPolyList(XmlTextWriter writer, Mesh mesh, string objectId)
        {
            writer.WriteStartElement("polylist");
            writer.WriteAttributeString("material", "Material-material");
            writer.WriteAttributeString("count", (mesh.Indices.Length / 3).ToString());

            writer.WriteStartElement("input");
            writer.WriteAttributeString("semantic", "VERTEX");
            writer.WriteAttributeString("source", "#" + objectId + "-mesh-vertices");
            writer.WriteAttributeString("offset", "0");
            writer.WriteEndElement();

            //writer.WriteStartElement("input");
            //writer.WriteAttributeString("semantic", "NORMAL");
            //writer.WriteAttributeString("source", "#" + objectId + "-mesh-normals");
            //writer.WriteAttributeString("offset", "1");
            //writer.WriteEndElement();

            writer.WriteStartElement("vcount");
            for (int i = 0; i < mesh.Indices.Length; i += 3)
            {
                writer.WriteString("3 ");
            }
            writer.WriteEndElement();

            writer.WriteStartElement("p");
            for (int i = 0; i < mesh.Indices.Length; i++)
            {
                writer.WriteString(mesh.Indices[i] + " ");
                //writer.WriteString(mesh.NormalIndices[i] + " ");
            }
            writer.WriteEndElement();

            writer.WriteEndElement(); //polylist
        }

        private static void SetLibraryVisualScenes(XmlTextWriter writer, List<Mesh> meshes)
        {
            writer.WriteStartElement("library_visual_scenes");
            writer.WriteStartElement("visual_scene");
            writer.WriteAttributeString("id", "Scene");
            writer.WriteAttributeString("name", "Scene");

            writer.WriteStartElement("node");
            writer.WriteAttributeString("id", "Camera");
            writer.WriteAttributeString("name", "Camera");
            writer.WriteAttributeString("type", "NODE");

            writer.WriteStartElement("matrix");
            writer.WriteAttributeString("sid", "transform");
            writer.WriteString("0.6858805 -0.3173701 0.6548619 7.481132 0.7276338 0.3124686 -0.6106656 -6.50764 -0.01081678 0.8953432 0.4452454 5.343665 0 0 0 1");
            writer.WriteEndElement(); //matrix

            writer.WriteStartElement("instance_camera");
            writer.WriteAttributeString("url", "#Camera-camera");
            writer.WriteEndElement();

            writer.WriteEndElement(); //node

            writer.WriteStartElement("node");
            writer.WriteAttributeString("id", "Lamp");
            writer.WriteAttributeString("name", "Lamp");
            writer.WriteAttributeString("type", "NODE");

            writer.WriteStartElement("matrix");
            writer.WriteAttributeString("sid", "transform");
            writer.WriteString("-0.2908646 -0.7711008 0.5663932 4.076245 0.9551712 -0.1998834 0.2183912 1.005454 -0.05518906 0.6045247 0.7946723 5.903862 0 0 0 1");
            writer.WriteEndElement(); //matrix

            writer.WriteStartElement("instance_light");
            writer.WriteAttributeString("url", "#Lamp-light");
            writer.WriteEndElement();

            writer.WriteEndElement(); //node

            for (int i = 0; i < meshes.Count; i++)
            {
                var objectId = "Object" + i;
                writer.WriteStartElement("node");
                writer.WriteAttributeString("id", objectId);
                writer.WriteAttributeString("name", objectId);
                writer.WriteAttributeString("type", "NODE");

                writer.WriteStartElement("matrix");
                writer.WriteAttributeString("sid", "transform");
                writer.WriteString("1 0 0 0 0 1 0 0 0 0 1 0 0 0 0 1");
                writer.WriteEndElement();

                writer.WriteStartElement("instance_geometry");
                writer.WriteAttributeString("url", "#" + objectId + "-mesh");

                writer.WriteStartElement("bind_material");
                writer.WriteStartElement("technique_common");
                writer.WriteStartElement("instance_material");
                writer.WriteAttributeString("symbol", "Material-material");
                writer.WriteAttributeString("target", "#Material-material");

                writer.WriteEndElement(); //instance_material
                writer.WriteEndElement(); //technique_common
                writer.WriteEndElement(); //bind_material
                writer.WriteEndElement(); //instance_geometry

                writer.WriteEndElement(); //node
            }

            writer.WriteEndElement(); //visual_scene
            writer.WriteEndElement(); //library_visual_scenes
        }
    }
}
