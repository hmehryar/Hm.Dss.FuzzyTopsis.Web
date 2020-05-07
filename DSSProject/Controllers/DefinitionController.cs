using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DSS.Views
{
    //[Authorize]
    public class DefinitionController : Controller
    {
		public DefinitionController()
		{

		}
        // GET: Definition
        
        public ActionResult Index()
        {
			System.Collections.ArrayList mineList = new System.Collections.ArrayList();
			mineList.Add(1);
			mineList.Add(2);
			mineList.Add(3);
			mineList.Add(4);

			ViewBag.mines = new SelectList(mineList);
			
			System.Collections.ArrayList methodList = new System.Collections.ArrayList();
			methodList.Add("TOPSIS");
			methodList.Add("Fuzzy TOPSIS");
			methodList.Add("AHP");
			methodList.Add("Fuzzy AHP");
			
			ViewBag.methods = new SelectList(methodList);

			var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            System.Collections.ArrayList fuzzyWeightList = new System.Collections.ArrayList();
            fuzzyWeightList.Add("Very Poor");
            fuzzyWeightList.Add("Poor");
            fuzzyWeightList.Add("Medium Poor");
            fuzzyWeightList.Add("Fair");
            fuzzyWeightList.Add("Medium Good");
            fuzzyWeightList.Add("Good");
            fuzzyWeightList.Add("Very Good");
			var c = oSerializer.Serialize(fuzzyWeightList);


			ViewBag.fuzzyWeight = c;
			// **********
			
			System.Collections.ArrayList fuzzyDecisionMatrixList = new System.Collections.ArrayList();
            fuzzyDecisionMatrixList.Add("Very Low");
            fuzzyDecisionMatrixList.Add("Low");
            fuzzyDecisionMatrixList.Add("Medium Low");
            fuzzyDecisionMatrixList.Add("Medium");
            fuzzyDecisionMatrixList.Add("Medium High");
            fuzzyDecisionMatrixList.Add("High");
            fuzzyDecisionMatrixList.Add("Very High");

			
			
			ViewBag.fuzzyDecisionMatrix = new SelectList (fuzzyDecisionMatrixList);
			//ViewBag.fuzzyWeight=[{"Very Low", "Low", "Medium Low", "Medium", "Medium High", "High", "Very High"}];               
			// **********
            

			return (View());
        }

        [HttpPost]
        ActionResult Decision()
        {
            return Content("");
        }

		//string method, int numCriteria, int numberOfAlternative, string[][] dgData, string[][] dgWeight
        //Models.FuzzyTopsis ft
        [HttpPost]
        public JsonResult Decision(Models.FuzzyTopsis ft)
		{
            //Models.FuzzyTopsis ft = new Models.FuzzyTopsis()
            //{
            //    dgData = dgData,
            //    dgWeight = dgWeight,
            //    numCriteria = numCriteria,
            //    numberOfAlternative = numberOfAlternative,
            //    method = method
            //};
			FuzzyTOPSIS FT = new FuzzyTOPSIS();
			List<Element[]> wd = new List<Element[]>();
			List<Element[][]> fdm = new List<Element[][]>();
           
			#region Init
			Element[] weight = new Element[ft.numCriteria];
			for (int i = 0; i < ft.numCriteria; i++)
				weight[i] = new Element();

			Element[][] l = new Element[ft.numberOfAlternative+1][];
			for (int i = 0; i < l.Length; i++)
			{
				l[i] = new Element[ft.numCriteria];
				for (int j = 0; j < ft.numCriteria; j++)
					l[i][j] = new Element();
			}
			#endregion

			List<Element> list = new List<Element>();
			Element el = new Element();

			for (int i = 0; i < ft.numberOfAlternative; i++)
			{
				for (int j = 0; j < ft.numCriteria; j++)
				{
					l[i][j].e = Find(ft.dgData[i][j]);
				}
			}


			for (int i = 0; i < ft.numCriteria; i++)
			{
				weight[i].e = Find(ft.dgWeight[i][0]);

			}

			wd.Add(weight);
			fdm.Add(l);


			int[] criteria = new int[ft.numCriteria];
			for (int i = 0; i < criteria.Length; i++)
			{
				criteria[i] = 1;
			}

			double[] result = FT.Do(wd, fdm, 1, ft.numCriteria, ft.numberOfAlternative, 1, criteria);

			double[] temp = new double[result.Length];
			for (int i = 0; i < result.Length; i++)
			{
				temp[i] = result[i] / result.Sum();
			}
			int[] alters = new int[ft.numberOfAlternative];
			for (int i = 0; i < result.Length; i++)
			{
				alters[i] = i;
			}

			Models.Result r=new Models.Result();
			r.data = temp;
			//var key = new System.Web.Helpers.Chart(width: 300, height: 300)
			//.AddTitle("نتیجه تصمیم گیری")
			//.AddSeries(
			//			chartType: "Bubble",
			//			name: "گزینه ها",
			//			xValue:alters ,
			//			//xValue: new[] { "Peter", "Andrew", "Julie", "Dave" },
			//			yValues:result
			//			//yValues: new[] { "2", "7", "5", "3" }
			//			);

			//return File(key.ToWebImage().GetBytes(), "image/jpeg");
			//return (View
			//			(viewName: "Result",
			//			model: r));
			return Json(r.data, JsonRequestBehavior.AllowGet);
			//string[][] dgData=null;
			//string[][] dgWeight = null;
			//#region Init
			//Element[] weight = new Element[numCriteria];
			//for (int i = 0; i < numCriteria; i++)
			//	weight[i] = new Element();

			//Element[][] l = new Element[numberOfAlternative][];
			//for (int i = 0; i < l.Length; i++)
			//{
			//	l[i] = new Element[numCriteria];
			//	for (int j = 0; j < numCriteria; j++)
			//		l[i][j] = new Element();
			//}
			//#endregion

			//List<Element> list = new List<Element>();
			//Element el = new Element();

			//for (int i = 0; i < numberOfAlternative - 1; i++)
			//{
			//	for (int j = 0; j < numCriteria; j++)
			//	{
			//		l[i][j].e = Find(dgData[i][j]);
			//	}
			//}


			//for (int i = 0; i < numCriteria; i++)
			//{
			//	weight[i].e = Find(dgWeight[0][i]);

			//}

			//wd.Add(weight);
			//fdm.Add(l);


			//int[] criteria = new int[numCriteria];
			//for (int i = 0; i < criteria.Length; i++)
			//{
			//	criteria[i] = 1;
			//}

			//double[] result = FT.Do(wd, fdm, 1, numCriteria, numberOfAlternative-1, 1, criteria);

			//for (int i = 0; i < result.Length; i++)
			//{
			//	result[i] = result[i] / result.Sum();
			//}
			//int[] alters=new int[numberOfAlternative];
			//for (int i=0;i<result.Length;i++){
			//	alters[i]=i;
			//}
		}
		private double[] Find(string str)
		{
			FuzzyTOPSIS FT = new FuzzyTOPSIS();
			Dictionary<string, double[]> dic = new Dictionary<string, double[]>();

			dic.Add("Very Low", FT.VL.ToArray<double>());
			dic.Add("Low", FT.L.ToArray<double>());
			dic.Add("Medium Low", FT.ML.ToArray<double>());
			dic.Add("Medium", FT.M.ToArray<double>());
			dic.Add("Medium High", FT.MH.ToArray<double>());
			dic.Add("High", FT.H.ToArray<double>());
			dic.Add("Very High", FT.VH.ToArray<double>());
			dic.Add("Very Poor", FT.VP.ToArray<double>());
			dic.Add("Poor", FT.P.ToArray<double>());
			dic.Add("Medium Poor", FT.MP.ToArray<double>());
			dic.Add("Fair", FT.F.ToArray<double>());
			dic.Add("Medium Good", FT.MG.ToArray<double>());
			dic.Add("Good", FT.G.ToArray<double>());
			dic.Add("Very Good", FT.VG.ToArray<double>());
			Element el = new Element();
			dic.TryGetValue(str, out el.e);
			return el.e;
		}
    }
}