using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics;
using System.Threading.Tasks;

namespace ACG_labs
{
    public class ModelInfo{
        public readonly string modelFolder;
        public readonly string modelPref;
        public readonly string diffPref;
        public readonly string specPref;
        public readonly string normalsPref;
        //если readonly глючит, то просто его убери

        public ModelInfo(string folder, string fmodel, string fdiff, string fspec, string fnorm){
            this.modelFolder = folder;
            this.diffPref = fdiff;
            this.modelPref = fmodel;
            this.specPref = fspec;
            this.normalsPref = fnorm;
        }
    }

    public static class AllModels{
        private static List<ModelInfo> models;
        private static int curr_model;
        public static void initModels(){
            ModelInfo head =new ModelInfo(Path.Combine("C:\\Users\\User\\source\\AKG\\AСG_labs", "Head"),"/model.obj","/diffuse.png","/specular.png","/normal.png");
            ModelInfo dolphin =new ModelInfo(Path.Combine("C:\\Users\\User\\source\\AKG\\AСG_labs", "Dolphin"), "/dolphin.obj","","","");
            ModelInfo plane = new ModelInfo(Path.Combine("C:\\Users\\User\\source\\AKG\\AСG_labs", "Plane"), "/model.obj", "/mapDiffuse.png", "/mapSpecular.png", "/mapNormal.png");
            models = new List<ModelInfo>();
            models.Add(head);
            models.Add(dolphin);
            models.Add(plane);
            curr_model = 1;
        }
        public static ModelInfo getModel(){
            return models[curr_model];
        }
        public static ModelInfo setModel(int new_model){
            curr_model = new_model >= models.Count ? models.Count-1 :new_model;
            if(curr_model < 0)
            curr_model = 0;
            return getModel();
        }
    }
}