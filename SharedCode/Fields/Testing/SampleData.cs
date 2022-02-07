#region + Using Directives

using System.Collections.Generic;
using SharedCode.Fields.ExStorage.ExStorManagement;
using static SharedCode.Fields.ExStorage.ExStorManagement.ExStoreStartRtnCodes;
using static SharedCode.Fields.ExStorage.ExStorManagement.ExStoreRtnCodes;
using static SharedCode.Fields.ExStorage.ExStorManagement.ExStoreSelectDsRtnCodes;

using SharedCode.Windows;

#endregion

// user name: jeffs
// created:   8/29/2021 7:02:18 AM

namespace SharedCode.Fields.Testing
{
	public static class SampleData
	{
		public struct testInfo
		{
			public int index { get; set; }
			public string routine { get; set; }
			public string description { get; set; }

			public testInfo(int index, string routine, string description)
			{
				this.index = index;
				this.routine = routine;
				this.description = description;
			}
		}


		public static AWindow W { get; set; }

		static SampleData()
		{
			makeStartData();
		}

		private static int idx = 0;

		public static int xxx = idx++; // filler for blank lines

		public static int f11 = idx++; //
		public static int f12 = idx++; //
		public static int f13 = idx++; //

		public static int f20 = idx++; // (read data)                0=fail,         1=good
		public static int f40 = idx++; // (ok to proceed)            0=fail,         1=good
		public static int f41 = idx++; // (confirm)                  0=fail,         1=good
		public static int f50 = idx++; // (request data)             0=fail/confirm, 1=good/ok, 2=cancel
		public static int f51 = idx++; // (save data)                0=fail,         1=good
		public static int f60 = idx++; // (erase these DS)           0=fail,         1=good
		public static int f70 = idx++; // (use old or new DS)        0=cancel,       1=new,     2=existing,  3=current
		public static int f71 = idx++; // (select, which DS)         0=cancel,       1=good
		public static int f73 = idx++; // (make DS, save, and store) 0=fail,         1=good

		public static int fx10 = idx++; //
		public static int fx30 = idx++; //
		public static int fx50 = idx++; //

		public static int p00 = idx++; // (procedure 00)
		public static int p01 = idx++; // (procedure 01)
		public static int p101 = idx++; // (procedure 201)
		public static int p201 = idx++; // (procedure 201)
		public static int p301 = idx++; // (procedure 301)
		public static int p801 = idx++; // (procedure 801)
		public static int p901 = idx++; // (procedure 901)
		public static int p903 = idx++; // (procedure 903)
		public static int p904 = idx++; // (procedure 904)

		public static int pX101 = idx++; // (procedure pX101)
		public static int pX201 = idx++; // (procedure pX201)
		public static int pX202 = idx++; // (procedure pX202)
		public static int pX301 = idx++; // (procedure pX301)
		public static int pX302 = idx++; // (procedure pX302)
		public static int pX401 = idx++; // (procedure pX401)
		public static int pX501 = idx++; // (procedure pX501)
		

		public static int pUx101 = idx++; // (procedure pXUx101)


		public static Dictionary<int, testInfo> ti = new Dictionary<int, testInfo>()
		{
			{xxx, new testInfo(xxx,     " ", "")},

			{f11, new testInfo(f11,     "fn11",   "get names")},
			{f12, new testInfo(f12,     "fn12",   "get list of DS")},
			{f13, new testInfo(f13,     "fn13",   "process list of DS")},

			{f20, new testInfo(f20,     "fn20",   "read data")},
			// don't need:  fn30

			{f40, new testInfo(f40,     "fn40",   "ok to proceed")},
			{f41, new testInfo(f41,     "fn41",   "don't exit")},
			{f50, new testInfo(f50,     "fn50",   "request data")},
			{f51, new testInfo(f51,     "fn51",   "save data")},
			{f60, new testInfo(f60,     "fn60",   "erase these DS")},
			{f70, new testInfo(f70,     "fn70",   "use exist or new DS")},
			{f71, new testInfo(f71,     "fn71",   "select exist DS to use")},
			{f73, new testInfo(f73,     "fn73",   "make DS and save/store")},

			{fx10, new testInfo(fx10,   "fx10",   "make lock data")},
			{fx30, new testInfo(fx30,   "fx30",   "get lock")},
			{fx50, new testInfo(fx50,   "fx50",   "verify-erase lock-ok")},

			{p00, new testInfo(p00,     "proc00", "prime procedure")},
			{p01, new testInfo(p01,     "proc01", "root/get DS")},
			{p101, new testInfo(p101,   "proc101", "get data DS")},
			{p201, new testInfo(p201,   "proc201", "will modify the model-OK?")},
			{p301, new testInfo(p301,   "proc301", "request data")},
			{p801, new testInfo(p801,   "proc801", "found current DS + other DS")},
			{p901, new testInfo(p901,   "proc901", "no DS + found other DS")},
			{p903, new testInfo(p903,   "proc903", "select DS to use")},
			{p904, new testInfo(p904,   "proc904", "select exist DS to use")},

			{pX101, new testInfo(pX101, "procX101", "create DS lock")},
			{pX201, new testInfo(pX201, "procX201", "remove DS lock-public")},
			{pX202, new testInfo(pX202, "procX202", "remove DS lock-private")},
			{pX301, new testInfo(pX301, "procX301", "get DS lock-public")},
			{pX302, new testInfo(pX302, "procX302", "get DS lock-private")},
			{pX401, new testInfo(pX401, "procX401", "check lock status")},
			{pX501, new testInfo(pX501, "procX501", "remove lock DS override")},
			

		};

		public static int tests { get; }  = 7;
		public static int firstTest = 4;


		private static int primLoops = 3;
		private static int responses = idx;
		private static int procIdx = 3;
		private static int procLoop = 3;

		public static int fn41p201 = 0;
		public static int fn41p301 = 1;
		public static int fn41p903 = 2;

		public static int fn60p801 = 0;
		public static int fn60p903 = 1;
		public static int fn60p904 = 2;

		public static int fn70p903 = 0;

		public static int fn73px101 = 0;
		public static int fn73p201  = 1;
		public static int fn73p903  = 2;



		public static string Address { get; set; }
		public static string Location { get; set; }

		public static int TestIdx { get; set; }
		public static int PrimeLoopIdx { get; set; }
		public static int LoopCnt { get; set; }

		public static int[] LoopIdx { get; set; } = new int[responses];

		public static string[] TestNames { get; private set; }
			= new [] {
				"test 1 / simple read / DS found / data found / data read / data shown", 
				"test 2 / DS found / data not found / request data / data read / data shown", 
				"test 3 / DS found / data not found / request data -> cancel / confirm -> yes / exit", 
				"test 4 / DS not found / request data / save data / show data", 
				"test 5 / DS not found + other DS found / use new", 
				"test 6 / DS not found + other DS found / use existing", 
				"test 7 / DS not found / don't modify / do modify / make data / etc.", 
				"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""};

		public static int[,,,,] SX = new int[tests, primLoops, responses, procIdx, procLoop];

		private static void makeStartData()
		{
			for (int i = 0; i < tests; i++)
			{
				for (int j = 0; j < primLoops; j++)
				{
					for (int k = 0; k < responses; k++)
					{
						for (int l = 0; l < procIdx; l++)
						{
							for (int m = 0; m < procLoop; m++)
							{
								SX[i, j, k, l, m] = -1;
							}
						}
					}
				}
			}

			int test = 0;
			
			// test 1
			SX[test, 0, p101, 0, 0] = (int) XSC_YES;
			SX[test, 0, f20, 0, 0] = (int) XRC_GOOD;
			
			test++;
			// test 2
			SX[test, 0, p101, 0, 0] = (int) XSC_YES;
			SX[test, 0, f20, 0, 0] = (int) XRC_FAIL;
			SX[test, 0, f50, 0, 0] = (int) XRC_FAIL;
			SX[test, 0, f41, fn41p301, 0] = (int) XRC_GOOD;
			SX[test, 0, f50, 0, 1] = (int) XRC_GOOD;
			SX[test, 0, f51, 0, 0] = (int) XRC_GOOD;
			SX[test, 1, p101, 0, 0] = (int) XSC_YES;
			SX[test, 1, f20, 0, 0] = (int) XRC_GOOD;
			
			test++;
			
			// test 3
			SX[test, 0, p101, 0, 0] = (int) XSC_YES;
			SX[test, 0, f20, 0, 0] = (int) XRC_FAIL;
			SX[test, 0, f50, 0, 0] = (int) XRC_FAIL;
			SX[test, 0, f41, fn41p301, 0] = (int) XRC_FAIL;
			
			test++;

			// test 4
			SX[test, 0, p101, 0, 0] = (int) XSC_NO;
			SX[test, 0, f40, 0, 0] = (int) XRC_GOOD;
			SX[test, 0, f73, 0, 0] = (int) XRC_GOOD;
			SX[test, 0, f20, 0, 0] = (int) XRC_FAIL;
			SX[test, 0, f50, 0, 0] = (int) XRC_FAIL;
			SX[test, 0, f41, fn41p301, 0] = (int) XRC_GOOD;
			SX[test, 0, f50, 0, 1] = (int) XRC_GOOD;
			SX[test, 0, f51, 0, 0] = (int) XRC_GOOD;
			// next loop
			SX[test, 1, p101, 0, 0] = (int) XSC_YES;
			SX[test, 1, f20, 0, 0] = (int) XRC_GOOD;



			test++;

			// test 5
			SX[test, 0, p101, 0, 0] = (int) XSC_NO_WITH_PRIOR;
			SX[test, 0, f70, fn70p903, 0] = (int) XDS_CANCEL;
			SX[test, 0, f41, fn41p903, 0] = (int) XRC_GOOD;
			SX[test, 0, f70, fn70p903, 1] = (int) XDS_USE_NEW;
			SX[test, 0, f60, fn60p903, 0] = (int) XRC_GOOD;
			SX[test, 0, f73, fn73p903, 0] = (int) XRC_GOOD;
			SX[test, 0, f20, 0, 0] = (int) XRC_FAIL;
			SX[test, 0, f50, 0, 0] = (int) XRC_GOOD;
			SX[test, 0, f51, 0, 0] = (int) XRC_GOOD;

			SX[test, 1, p101, 0, 0] = (int) XSC_YES;
			SX[test, 1, f20, 0, 0] = (int) XRC_GOOD;

			test++;

			// test 6
			SX[test, 0, p101, 0, 0] = (int) XSC_NO_WITH_PRIOR;
			SX[test, 0, f70, fn70p903, 0] = (int) XDS_CANCEL;
			SX[test, 0, f41, fn41p903, 0] = (int) XRC_GOOD;
			SX[test, 0, f60, fn60p904, 0] = (int) XRC_GOOD;
			SX[test, 0, f70, fn70p903, 1] = (int) XDS_USE_EXIST;
			SX[test, 0, f71, 0, 0] = (int) XRC_GOOD;
			SX[test, 0, f20, 0, 0] = (int) XRC_FAIL;
			SX[test, 0, f50, 0, 0] = (int) XRC_FAIL;
			SX[test, 0, f41, fn41p301, 0] = (int) XRC_GOOD;
			SX[test, 0, f50, 0, 1] = (int) XRC_FAIL;
			SX[test, 0, f41, fn41p301, 1] = (int) XRC_GOOD;
			SX[test, 0, f50, 0, 2] = (int) XRC_GOOD;
			SX[test, 0, f51, 0, 0] = (int) XRC_GOOD;

			SX[test, 1, p101, 0, 0] = (int) XSC_YES;
			SX[test, 1, f20, 0, 0] = (int) XRC_GOOD;

						
			test++;

			// test 7
			SX[test, 0, p101, 0, 0] = (int) XSC_NO;
			SX[test, 0, f40, 0, 0] = (int) XRC_FAIL;
			SX[test, 0, f41, 0, 0] = (int) XRC_GOOD;
			SX[test, 0, f40, 0, 1] = (int) XRC_GOOD;
			SX[test, 0, f73, 0, 0] = (int) XRC_GOOD;
			SX[test, 0, f20, 0, 0] = (int) XRC_FAIL;
			SX[test, 0, f50, 0, 0] = (int) XRC_FAIL;
			SX[test, 0, f41, fn41p301, 0] = (int) XRC_GOOD;
			SX[test, 0, f50, 0, 1] = (int) XRC_GOOD;
			SX[test, 0, f51, 0, 0] = (int) XRC_GOOD;
			// next loop
			SX[test, 1, p101, 0, 0] = (int) XSC_YES;
			SX[test, 1, f20, 0, 0] = (int) XRC_GOOD;


		}

		public static ExStoreStartRtnCodes[] rtnCode0 { get; } = new []
			{XSC_NO, XSC_NO_WITH_PRIOR, XSC_YES, XSC_YES_WITH_PRIOR};

		public static ExStoreRtnCodes[] rtnCode1 { get; } = new [] 
			{XRC_FAIL, XRC_GOOD, XRC_CANCEL};

		public static ExStoreSelectDsRtnCodes[] rtnCode2 { get; } = new []
			{XDS_CANCEL, XDS_USE_NEW, XDS_USE_EXIST, XDS_USE_CURRENT};


		public static void SetPrimeLoop(int idx)
		{
			PrimeLoopIdx = idx;
		}

		public static void SetSubLoop(int idx, int subIdx)
		{
			LoopIdx[idx] = subIdx;
		}

		public static ExStoreStartRtnCodes RtnCode00(int location, int idx, int procIdx = 0)
		{
			int result = calcIdx(location, idx, procIdx);

			// sayResult(rtnCode0[result].ToString(), location, idx);
		
			return rtnCode0[result];
		}

		
		public static ExStoreRtnCodes RtnCode01(int location, int idx, int procIdx = 0)
		{
			int result = calcIdx(location, idx, procIdx);

			// sayResult(rtnCode1[result].ToString(), location, idx);

			return rtnCode1[result];
		}

		public static ExStoreSelectDsRtnCodes RtnCode02(int location, int idx, int procIdx = 0)
		{
			int result = calcIdx(location, idx, procIdx);

			// sayResult(rtnCode2[result].ToString(), location, idx);

			return rtnCode2[result];
		}

		private static void sayResult(string result, int location, int itemIndex)
		{

			W.WriteLineAligned($"at {ti[itemIndex].routine}  ({ti[location].routine})", null, $"=> {result}  ({ti[itemIndex].description})");
			// W.WriteLineAligned("get result| ", $"{result}  [{ti[location].routine} | {ti[itemIndex].routine}]");
		}

		private static int calcIdx(int location, int itemIdx, int procIdx)
		{
			Location = $"{ti[location].routine} | {ti[itemIdx].routine}";
			Address = $"{(TestIdx + 1)} : {PrimeLoopIdx} : {itemIdx} ({ti[itemIdx].routine}) : {procIdx} : {LoopIdx[itemIdx]}";

			int result = SX[TestIdx, PrimeLoopIdx, itemIdx, procIdx, LoopIdx[itemIdx]];

			return result;
		}

	}
}