#region + Using Directives

using SharedCode.Windows;
using SharedCode.Fields.ExStorage.ExStorManagement;
using SharedCode.Fields.Testing;
using SharedCode.ShowInformation;
using UtilityLibrary;

// using CSToolsDelux.WPF;
// using CSToolsDelux.Fields.Testing;
// using CSToolsDelux.Fields.ExStorage.DataStorageManagement;

#endregion

// user name: jeffs
// created:   9/18/2021 10:36:43 AM

namespace CSToolsStudies.FieldsManagement
{
	public class FieldsStartProcedure
	{
		// private ExStoreManager exMgr;
		private ShShowInfo show;

		public FieldsStartProcedure(AWindow w)
		{
			show = new ShShowInfo(w, CsUtilities.AssemblyName, "CsToolsStudies");
		}

	#region pseudo code routines

		// initial 
		// proc01
		public ExStoreRtnCodes DoesDataStoreExist()
		{
			int location = SampleData.p01;
			int loopCnt = 0;
			bool repeat = true;
			int subIdx10 = 0;
			int subIdx20 = 0;
			
			// show.informStart(SampleData.xxx, "", "");

			show.informStartEnter(location, "entering main routine");

			// show.informStart(SampleData.p01, "entering", "->", " ");

			ExStoreRtnCodes result = ExStoreRtnCodes.XRC_FAIL;

			do
			{
				// SampleData.SetSubLoop(SampleData.f10, subIdx10++);
				// SampleData.SetSubLoop(SampleData.f20, subIdx20++);

				show.informStartLoopBeg(location, $"top of loop| loop count| {loopCnt}", $"{loopCnt} -------------v");

				// show.informStart(location, $"top of loop| loop count| {loopCnt}", " *>", " ");

				SampleData.SetPrimeLoop(loopCnt++);
				// determine if DS exists / save if found
				// fn10
				ExStoreStartRtnCodes choice = getExistDataStorage(location);

				switch (choice)
				{
				case ExStoreStartRtnCodes.XSC_YES: // exists + NO other data stores
					{
						// yes
						result = ExStoreRtnCodes.XRC_GOOD;
						break;
					}
				case ExStoreStartRtnCodes.XSC_YES_WITH_PRIOR: 
					{
						// yes+
						// exists + other DS found
						result = proc801();
						break;
					}
				case ExStoreStartRtnCodes.XSC_NO: 
					{
						// no
						// NOT exists + NO other DS found
						result = proc201();
						break;
					}
				case ExStoreStartRtnCodes.XSC_NO_WITH_PRIOR: 
					{
						// no+
						// not exists + other DS found
						result = proc901();
						break;
					}
				}

				if (result == ExStoreRtnCodes.XRC_FAIL)
				{
					repeat = false;
				}
				else
				{
					//fn20
					result = fn20(location);
					// result = SampleData.RtnCode01(SampleData.p01, SampleData.f20);
					if (result == ExStoreRtnCodes.XRC_GOOD)
					{
						repeat = false;
					}
					else
					{
						// got fail
						result = proc301();
						if (result == ExStoreRtnCodes.XRC_FAIL) repeat = false;
					}

				}
				// if fail, exit loop and return fail
				// if good, now got data, repeat for proper processing
				show.informStartLoopEnd(location, $"bottom of loop| repeat loop?| {repeat}",
					repeat ? $"{(loopCnt-1)} ------Y------^" : $"{(loopCnt-1)} ------N------^");
					// repeat ? "loop again" : "loop exit");
				// show.informStart(location, $"bottom of loop| repeat loop?| {repeat}", " <*");
			}
			while (repeat);


			show.informStartExit(location, "exiting", result.ToString());
			// show.informStart( SampleData.p01, "exiting", "<-", result.ToString());
			show.informStart(SampleData.xxx, "", "");
			return result;
		}

		// get the existing DS
		// fn10
		// proc101
		public ExStoreStartRtnCodes getExistDataStorage(int location)
		{
			ExStoreStartRtnCodes result;

			// return 
			// yes, no, yes+, no+

			// if yes or yes+
			// store DS

			int op = SampleData.p101;

			result = SampleData.RtnCode00(location, op);

			show.informStartCont(op, result.ToString());
			// show.informStart(op, location, "    v", result.ToString());

			return result;
		}


		private ExStoreRtnCodes proc201()
		{
			int location = SampleData.p201;
			int subIdx41 = 0;
			SampleData.SetSubLoop(SampleData.f41, subIdx41);

			ExStoreRtnCodes result;

			show.informStartEnter(location, "entering");
			// show.informStart( SampleData.p201, "entering", "->", " ");

			// ok to modify the model, ok?
			result = fn40(location);

			if (result == ExStoreRtnCodes.XRC_FAIL)
			{
				// not ok to modify -> verify
				result = fn41(location, SampleData.fn41p201);
				if (result == ExStoreRtnCodes.XRC_FAIL)
				{
					show.informStartExit(location, "exiting", result.ToString());
					// show.informStart( SampleData.p201, "exiting", "<-", result.ToString());
					return result;
				}
				// changed mind, ok to modify
			}

			// got good -> ok to modify
			// make a DS
			result = fn73(location, SampleData.fn73p201);

			show.informStartExit(location, "exiting", result.ToString());
			// show.informStart( SampleData.p201, "exiting", "<-", result.ToString());
			return result;
		}


		// request data
		// proc301
		private ExStoreRtnCodes proc301()
		{
			int location = SampleData.p301;
			int loopCnt = 0;
			int subIdx41 = 0;
			int subIdx50 = 0;
			
			show.informStartEnter(location, "entering");
			// show.informStart( SampleData.p301, "entering", "->", " ");

			bool repeat = true;
			ExStoreRtnCodes result;

			do
			{
				show.informStartLoopBeg(location, "top of loop", $"{loopCnt} --------v");

				// show.informStart(location, $"top of loop", " *>", " ");
				SampleData.SetSubLoop(SampleData.f41, subIdx41++);
				SampleData.SetSubLoop(SampleData.f50, subIdx50++);

				// show dialog, get data
				result = fn50(location);
				if (result == ExStoreRtnCodes.XRC_FAIL)
				{
					result = fn41(location, SampleData.fn41p301);
					if (result == ExStoreRtnCodes.XRC_FAIL)
					{
						show.informStartLoopEnd(location, "exit loop", $"{loopCnt} ---x----^");
						// show.informStart(location, $"exit loop", " <x", " ");

						show.informStartExit(location, "exiting", result.ToString());
						// show.informStart( SampleData.p301, "exiting", "<-", result.ToString());
						return result;
					}
				}
				else
				{
					repeat = false;
				}

				show.informStartLoopEnd(location, $"bottom of loop| repeat loop?| {repeat}", 
					repeat ? $"{loopCnt} ---Y----^" : $"{loopCnt} ---N----^");
				// repeat ? "loop again" : "loop exit");
				// show.informStart(location, $"bottom of loop| repeat loop?| {repeat}", " <*", " ");
			}
			while (repeat);
			
			// got data, save data
			result = fn51(location);

			show.informStartExit(location, "exiting", result.ToString());
			// show.informStart( SampleData.p301, "exiting", "<-", result.ToString());
			return result;
		}

		// found current DS + prior DS
		// this should never happen
		// proc801
		private ExStoreRtnCodes proc801()
		{
			int location = SampleData.p801;

			show.informStartEnter(location, "entering");
			// show.informStart( SampleData.p801, "entering", "->", " ");
			ExStoreRtnCodes result;

			// erase the prior DS
			result = fn60(location, SampleData.fn60p801);

			show.informStartExit(location, "exiting", result.ToString());
			// show.informStart( SampleData.p801, "exiting", "<-", result.ToString());
			return result;
		}

		// no DS found + found prior DS
		// this should never happen
		// proc901
		private ExStoreRtnCodes proc901()
		{
			int location = SampleData.p901;

			show.informStartEnter(location, "entering");
			// show.informStart( SampleData.p901, "entering", "->", " ");
			bool repeat = true;
			ExStoreRtnCodes result = ExStoreRtnCodes.XRC_FAIL;

			result = proc903();

			show.informStartExit(location, "exiting", result.ToString());
			// show.informStart( SampleData.p901, "exiting", "<-", result.ToString());
			return result;
		}



		// select a DS to use
		// proc903
		// rtn: good, fail
		private ExStoreRtnCodes proc903()
		{
			int location = SampleData.p903;
			int loopCnt = 0;
			int subIdx41 = 0;
			int subIdx70 = 0;
			int subIdx71 = 0;
			SampleData.SetSubLoop(SampleData.f71, subIdx71);

			show.informStartEnter(location, "entering");
			// show.informStart( SampleData.p903, "entering", "->", " ");
			bool repeat = true;
			ExStoreRtnCodes result = ExStoreRtnCodes.XRC_FAIL;
			ExStoreSelectDsRtnCodes answer;

			do
			{
				show.informStartLoopBeg(location, "top of loop", $"{loopCnt} --------v");
				// show.informStart(location, $"top of loop", " *>", " ");

				SampleData.SetSubLoop(SampleData.f41, subIdx41++);
				SampleData.SetSubLoop(SampleData.f70, subIdx70++);
				// inform, old DS found,
				// select which to use or make new
				answer = fn70(location);

				if (answer == ExStoreSelectDsRtnCodes.XDS_CANCEL) // exit
				{
					// confirm exit
					result = fn41(location, SampleData.fn41p903);
					if (result == ExStoreRtnCodes.XRC_FAIL)
					{
						show.informStartLoopEnd(location, "exit loop", $"{loopCnt} ---x----^");
						// show.informStart(location, $"exit loop", " <x", " ");

						show.informStartExit(location, "exiting", result.ToString());
						// show.informStart( SampleData.p903, "exiting", "<-", result.ToString());
						return result;
					}

					// got proceed: re-do & re-ask
				} 
				else if (answer == ExStoreSelectDsRtnCodes.XDS_USE_EXIST)
				{
					result = proc904();
					if (result != ExStoreRtnCodes.XRC_CANCEL)
					{
						show.informStartLoopEnd(location, "exit loop", $"{loopCnt} ---x----^");
						// show.informStart(location, $"exit loop", " <x", " ");

						show.informStartExit(location, "exiting", result.ToString());
						// show.informStart( SampleData.p903, "exiting", "<-", result.ToString());
						return result;
					}
					// got cancel: re-do & re-ask
				}
				else
				{
					// got new: exit loop
					repeat = false;
				}

				show.informStartLoopEnd(location, $"bottom of loop| repeat loop?| {repeat}",
					repeat ? $"{loopCnt} ---Y----^" : $"{loopCnt} ---N----^");
				// repeat ? "loop again" : "loop exit");
				// show.informStart(location, $"bottom of loop| repeat loop?| {repeat}", " <*", " ");
			}
			while (repeat);

			// got here: new

			// remove old
			result = fn60(location, SampleData.fn60p903);

			if (result == ExStoreRtnCodes.XRC_GOOD)
			{
				result = fn73(location, SampleData.fn73p903);
			}
			show.informStartExit(location, "exiting", result.ToString());
			// show.informStart( SampleData.p903, "exiting", "<-", result.ToString());
			return result;
		}

		// select exist DS to use
		// proc904
		// rtn: good, fail, cancel
		private ExStoreRtnCodes proc904()
		{
			int location = SampleData.p904;

			SampleData.SetSubLoop(SampleData.f71, 
				SampleData.LoopIdx[SampleData.f71]++);

			show.informStartEnter(location, "entering");
			// show.informStart( SampleData.p904, "entering", "->", " ");
			ExStoreRtnCodes result = ExStoreRtnCodes.XRC_FAIL;

			// select existing DS to use, store result
			result = fn71(location);
			if (result != ExStoreRtnCodes.XRC_GOOD)
			{
				show.informStartExit(location, "exiting", result.ToString());
				// show.informStart( SampleData.p904, "exiting", "<-", result.ToString());
				return result;
			}

			// delete other DS
			result = fn60(location, SampleData.fn60p904);

			show.informStartExit(location, "exiting", result.ToString());
			// show.informStart( SampleData.p904, "exiting", "<-", result.ToString());

			return result;
		}

/*
		// create the lock DS
		// procLX101
		// 
		private ExStoreRtnCodes procLX101(int location)
		{
			ExStoreRtnCodes result = ExStoreRtnCodes.XRC_FAIL;

			if (DataStoreManager.LockStatus != ExStorDsLockStatus.XLK_UNKNOWN) return result;
			
			fnX10();

			result = fn73(location, SampleData.fn73px101);
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			result = fn51(location);

			return result;
		}
*/





	#region psuedo methods

		// public ExStoreRtnCodes fnxx()
		// {
		// 	ExStoreRtnCodes result;
		//
		// 	return ExStoreRtnCodes.XRC_GOOD;
		// }


		public ExStoreRtnCodes fn20(int location)
		{
			ExStoreRtnCodes result;

			int op = SampleData.f20;

			result = SampleData.RtnCode01(location, op);

			show.informStartCont(op, result.ToString());

			// show.informStart(op, location, "    v", result.ToString());

			return result;
		}

		// ask: ok to proceed
		// fn40
		public ExStoreRtnCodes fn40(int location)
		{
			ExStoreRtnCodes result;

			int op = SampleData.f40;

			// proceed -> good
			// no -> fail
			result = SampleData.RtnCode01(location, op);

			show.informStartCont(op, result.ToString());
			// show.informStart(op, location, "    v", result.ToString());

			return result;
		}
		
		// confirm: if cancel, cannot use fields
		// fn41
		public ExStoreRtnCodes fn41(int location, int procIdx)
		{
			ExStoreRtnCodes result;

			int op = SampleData.f41;

			// proceed -> good
			// exit -> fail
			result = SampleData.RtnCode01(location, op, procIdx);

			show.informStartCont(op, result.ToString());
			// show.informStart(op, location, "    v", result.ToString());

			return result;
		}

		// request data
		// fn50
		public ExStoreRtnCodes fn50(int location)
		{
			ExStoreRtnCodes result;

			int op = SampleData.f50;

			// ok -> good
			// cancel -> fail
			result = SampleData.RtnCode01(location, op);

			show.informStartCont(op, result.ToString());
			// show.informStart(op, location, "    v", result.ToString());

			return result;
		}

		// save data
		// fn51
		public ExStoreRtnCodes fn51(int location)
		{
			ExStoreRtnCodes result;

			int op = SampleData.f51;

			// worked -> good
			// nope -> fail
			result = SampleData.RtnCode01(location, op);

			show.informStartCont(op, result.ToString());
			// show.informStart(op, location, "    v", result.ToString());

			return result;
		}


		// erase these DS (provide list)
		public ExStoreRtnCodes fn60(int location, int procIdx)
		{
			ExStoreRtnCodes result;

			int op = SampleData.f60;

			// worked -> good
			// nope -> fail
			result = SampleData.RtnCode01(location, op, procIdx);

			show.informStartCont(op, result.ToString());
			// show.informStart(op, location, "    v", result.ToString());

			return result;
		}

		// show.informStart: old DS found but no current
		// question: use an old or create a new?
		// fn70
		public ExStoreSelectDsRtnCodes fn70(int location)
		{
			ExStoreSelectDsRtnCodes result;

			int op = SampleData.f70;

			// new -> new
			// old -> old
			// current -> current
			// cancel -> cancel
			result = SampleData.RtnCode02(location,op);

			show.informStartCont(op, result.ToString());
			// show.informStart(op, location, "    v", result.ToString());

			return result;
		}

		
		// select which existing DS to use
		// fn71
		// rtn: good, cancel
		public ExStoreRtnCodes fn71(int location)
		{
			ExStoreRtnCodes result;

			int op = SampleData.f71;

			// cancel -> cancel
			// selected (saved in dsMgr) -> good
			result = SampleData.RtnCode01(location,op);

			show.informStartCont(op, result.ToString());
			// show.informStart(op, location, "    v", result.ToString());

			return result;
		}


		// make DS and save
		public ExStoreRtnCodes fn73(int location, int procIdx)
		{
			ExStoreRtnCodes result;

			int op = SampleData.f73;

			result = SampleData.RtnCode01(location, op, procIdx);

			show.informStartCont(op, result.ToString());
			// show.informStart(op, location, "    v", result.ToString());

			return result;
		}

		// make lock data
		// fnX10
		private void fnX10()
		{

		}



	#endregion


		

	#endregion

	}
}
