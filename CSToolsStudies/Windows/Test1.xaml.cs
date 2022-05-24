using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using CSToolsStudies.Annotations;
using UtilityLibrary;

namespace CSToolsStudies.Windows
{
	/// <summary>
	/// Interaction logic for Test1.xaml
	/// </summary>
	public partial class Test1 : Window, INotifyPropertyChanged
	{
		private bool isSelected = false;
		private bool? isEditing = false;
		private bool? noEditing = false;
		private bool? isLocked = false;

		private bool winLocationChanged = false;

		private int currPopupIdx = -1;
		private Popup[] popups = new Popup[10];
		private byte canEditPopup = 0;
		private byte action1Popup = 1;

		private bool popupIsEntered;
		private DoubleAnimation d = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(2)));
		private AnimationClock popupAniClock;

		private DispatcherTimer timer;
		private bool timerActive;

		// private Popup canEditPopup;

		public Test1()
		{
			InitializeComponent();
		}

		public bool IsSelected
		{
			[DebuggerStepThrough]
			get => isSelected;

			set
			{
				if (value == isSelected) return;
				isSelected = value;

				if (isSelected == false)
				{
					IsEditing = false;
					NoEditing = false;
				}

				OnPropertyChanged();
			}
		}

		public bool? IsEditing
		{
			[DebuggerStepThrough]
			get => isEditing;

			set
			{
				if (value == isEditing) return;

				if (isLocked.HasValue && isLocked.Value ||
					!isSelected)
				{
					isEditing = false;
				}
				else
				{
					isEditing = value;

					if (isEditing.HasValue && isEditing.Value)
					{
						noEditing = false;
					}
				}

				OnPropertyChanged();
				OnPropertyChanged(nameof(NoEditing));
			}
		}

		public bool? NoEditing
		{
			[DebuggerStepThrough]
			get => noEditing;

			set
			{
				if (value == noEditing) return;

				if (isLocked.HasValue && isLocked.Value
					|| !isSelected)
				{
					noEditing = false;
				}
				else
				{
					noEditing = value;

					if (noEditing.HasValue && noEditing.Value)
					{
						isEditing = false;
					}
				}

				OnPropertyChanged();
				OnPropertyChanged(nameof(IsEditing));
			}
		}

		public bool? IsLocked
		{
			[DebuggerStepThrough]
			get => isLocked;

			set
			{
				if (value == isLocked) return;
				isLocked = value;

				if (isLocked.HasValue == true && isLocked.Value)
				{
					IsEditing = false;
					NoEditing = false;
				}

				OnPropertyChanged();
			}
		}

		public bool WinLocationChanged
		{
			get => winLocationChanged;
			set
			{
				winLocationChanged = value;
				OnPropertyChanged();

			}
		}


		public event PropertyChangedEventHandler PropertyChanged;
		
		[NotifyPropertyChangedInvocator]
		[DebuggerStepThrough]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void BtnCanEditClear_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@EditClear");
		}





		private void win_Loaded(object sender, RoutedEventArgs e)
		{
			popups[canEditPopup] = CsWpfUtilities.FindElementByName<Popup>(this, "PuCanEdit");
		}


		private void win_LocationChanged(object sender, EventArgs e)
		{
			WinLocationChanged = true;

			// tracePathUp("location changed| enter");

			bpopupClose();

			// tracePathDn("location changed| exit");

		}




		private void bPopupCanEdit_MouseDown(object sender, MouseButtonEventArgs e)
		{
			// Debug.WriteLine("\n------------  mouse down -----------");
			//
			// depth = 0;
			// tracePathUp("mouse down| enter");
			if (currPopupIdx >= 0)
			{
				// tracePathDn("mouse down| exit if (idx >= 0)");
				return;
			}

			bpopupOpen(canEditPopup);

			popupIsEntered = false;

			bpopupStartClock();

			// tracePathDn("mouse down| exit");
		}

		private void bBtnPopupCanEdit_OnClick(object sender, RoutedEventArgs e)
		{
			// tracePathUp("BtnPopupClose_OnClick| enter");
			bpopupClose();

			// tracePathDn("BtnPopupClose_OnClick| exit");
		}

		private void bPopUpCanEdit_OnMouseLeave(object sender, RoutedEventArgs e)
		{
			// tracePathUp("Popup_OnMouseLeave| enter");
			
			popupIsEntered = false;

			bpopupStartClock();

			// tracePathDn("Popup_OnMouseLeave| exit");
		}

		private void bPopUpCanEdit_OnMouseEnter(object sender, RoutedEventArgs e)
		{
			// tracePathUp("Popup_OnMouseEnter| enter");

			// popupRemoveClock();

			popupIsEntered = true;

			// tracePathDn("Popup_OnMouseEnter| exit");
		}




		private void bpopupOpen(byte idx)
		{
			// tracePathUp($"popupOpen| enter| idx| {idx}  currIdx| {currPopupIdx}");

			bpopupClose();  // Close() and remove any clocks;

			currPopupIdx = idx;

			if (!popups[currPopupIdx].IsOpen) popups[currPopupIdx].IsOpen = true;

			// tracePathDn("popupOpen| exit");
		}

		private void bpopupClose()
		{
			// tracePathUp($"popupClose| enter| currIdx| {currPopupIdx}");

			if (currPopupIdx < 0)
			{
				// tracePathDn("popupClose| exit if (idx < 0)");
				return;
			}

			if (popups[currPopupIdx].IsOpen) popups[currPopupIdx].IsOpen = false;

			// removeClock(ref popupAniClock);

			currPopupIdx = -1;

			// tracePathDn("popupClose| exit");
		}







		private void bpopupRemoveClock()
		{
			// tracePathUp($"popupRemoveClock| enter| clock hash code| {(popupAniClock?.GetHashCode().ToString() ?? "is null")}");
			
			removeClock(ref popupAniClock);

			// string stat = clockStat(popupAniClock);

			// tracePathDn($"popupRemoveClock| exit| state| {stat}");
		}
		
		private void bpopupStartClock()
		{
			// tracePathUp($"popupStartClock| enter| clock hash code| {(popupAniClock?.GetHashCode().ToString() ?? "is null")}");

			startClock(ref popupAniClock);

			// string stat = clockStat(popupAniClock);

			// tracePathDn($"popupStartClock| exit| state| {stat}");
		}













		private void popupClockCompleted(object sender, EventArgs e)
		{
			// string stat = clockStat((AnimationClock) sender );

			// tracePathUp($"popupClockCompleted| enter| stat| {stat}");

			if (((AnimationClock) sender ).CurrentState == ClockState.Stopped)
			{
				// tracePathDn("popupClockCompleted| exit if| stopped");
				return;
			}

			bpopupRemoveClock();

			if (popupIsEntered)
			{
				// tracePathDn("popupClockCompleted| exit if| IsEntered");
				return;
			}

			bpopupClose();
			

			// tracePathDn("popupClockCompleted| exit");
		}

		private void removeClock(ref AnimationClock c)
		{
			// tracePathUp($"removeClock| enter| exist hashcode| {(c?.GetHashCode().ToString() ?? "is null")}");

			if (c == null)
			{
				// tracePathDn("removeClock| exit if (clock is null)");
				return;
			}

			c.Controller.Stop();
			c.Controller.Remove();
			c = null;

			// tracePathDn("removeClock| exit| ");
		}

		private void startClock(ref AnimationClock c)
		{
			// string stat = c == null ? "is null" : $"state| {c.CurrentState}  hash|{ c.GetHashCode()}";
			//
			// tracePathUp($"startClock| enter| stat| {stat}");

			if (c != null && c.GetHashCode() > 0)
			{
				// tracePathDn($"startClock| exit| if has hash");
				return;
			}

			c = d.CreateClock();

			c.Completed += popupClockCompleted;

			c.Controller.Begin();

			// tracePathDn($"startClock| exit| new hashcode|  {(c?.GetHashCode().ToString() ?? "is null")}");

		}

		private int depth = 0;


		private string clockStat(AnimationClock c)
		{
			return  c == null ? "is null" : $"state| {c.CurrentState}  hash|{ c.GetHashCode()}";
		}

		private void tracePathUp(string description)
		{
			tracePathWrite($"> {description}");
			depth += 1;
		}

		
		private void tracePathDn(string description)
		{
			depth -= 1;
			tracePathWrite($"< {description}");
			// depth -= 1;
		}

		private void tracePathWrite(string description)
		{
			if (depth < 0) depth = 0;

			Debug.Write("  ".Repeat(depth));
			Debug.WriteLine(description);
		}



		private void PopupAction1_MouseDown(object sender, MouseButtonEventArgs e)
		{
			// Debug.WriteLine("\n------------  mouse down -----------");
			//
			// depth = 0;
			// tracePathUp("mouse down| enter");
			if (currPopupIdx >= 0)
			{
				// tracePathDn("mouse down| exit if (idx >= 0)");
				return;
			}

			popupOpen(canEditPopup);

			popupIsEntered = false;

			startTimer();

			// tracePathDn("mouse down| exit");
		}


		
		private void PopupCanEdit_MouseDown(object sender, MouseButtonEventArgs e)
		{
			// Debug.WriteLine("\n------------  mouse down -----------");
			//
			// depth = 0;
			// tracePathUp("mouse down| enter");
			if (currPopupIdx >= 0)
			{
				// tracePathDn("mouse down| exit if (idx >= 0)");
				return;
			}

			popupOpen(canEditPopup);

			popupIsEntered = false;

			startTimer();

			// tracePathDn("mouse down| exit");
		}








		private void BtnPopupClose_OnClick(object sender, RoutedEventArgs e)
		{
			// tracePathUp("BtnPopupClose_OnClick| enter");

			
			popupClose();

			// tracePathDn("BtnPopupClose_OnClick| exit");
		}

		private void Popup_OnMouseLeave(object sender, RoutedEventArgs e)
		{
			// tracePathUp("Popup_OnMouseLeave| enter");
			
			popupIsEntered = false;

			startTimer();

			// tracePathDn("Popup_OnMouseLeave| exit");
		}

		private void Popup_OnMouseEnter(object sender, RoutedEventArgs e)
		{
			// tracePathUp("Popup_OnMouseEnter| enter");

			// popupRemoveClock();

			popupIsEntered = true;

			removeTimer();

			// tracePathDn("Popup_OnMouseEnter| exit");
		}




		private void popupOpen(byte idx)
		{
			// tracePathUp($"popupOpen| enter| idx| {idx}  currIdx| {currPopupIdx}");

			currPopupIdx = idx;

			if (!popups[currPopupIdx].IsOpen) popups[currPopupIdx].IsOpen = true;

			// tracePathDn("popupOpen| exit");
		}

		private void popupClose()
		{
			// tracePathUp($"popupClose| enter| currIdx| {currPopupIdx}");

			if (currPopupIdx < 0)
			{
				// tracePathDn("popupClose| exit if (idx < 0)");
				return;
			}

			if (popups[currPopupIdx].IsOpen) popups[currPopupIdx].IsOpen = false;

			// removeClock(ref popupAniClock);

			removeTimer();

			currPopupIdx = -1;

			// tracePathDn("popupClose| exit");
		}



		private void popupTimerCompleted(object sender, EventArgs e)
		{
			removeTimer();

			if (popupIsEntered)
			{
				// tracePathDn("popupClockCompleted| exit if| IsEntered");
				return;
			}

			popupClose();
		}

		private void removeTimer()
		{
			if (!timerActive) return;

			timer.Stop();

			timerActive = false;
		}


		private void startTimer()
		{
			if (timerActive) return;

			timer = new DispatcherTimer();
			timer.Interval = new TimeSpan(0, 0, 3);

			timerActive = true;

			timer.Tick += popupTimerCompleted;

			timer.Start();
		}





	}
}
