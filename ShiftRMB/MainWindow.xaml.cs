using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShiftRMB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Winmm.TIMECALLBACK m_func;
        int m_timer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            m_func = new Winmm.TIMECALLBACK(Timer_Elapsed);
            m_timer = Winmm.timeSetEvent(100, 1, m_func, IntPtr.Zero, 1/*TIME_PERIODIC*/);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Winmm.timeKillEvent(m_timer);
        }

        private void Timer_Elapsed(int uTimerID, int uMsg, IntPtr dwUser, IntPtr dw1, IntPtr dw2)
        {
            if ((User32.GetAsyncKeyState(0x91/*VK_SCROLL*/) & 0x0001) != 0)
            {
                if ((User32.GetKeyState(0x91/*VK_SCROLL*/) & 0x0001) != 0)
                {
                    if ((User32.GetAsyncKeyState(0x10/*VK_SHIFT*/) & 0x8000) == 0)
                    {
                        User32.SendInput(new User32.KEYBDINPUT
                        {
                            wVk = 0x10/*VK_SHIFT*/,
                            wScan = (short)User32.MapVirtualKey(0x10/*VK_SHIFT*/, 0/*MAPVK_VK_TO_VSC*/),
                            dwFlags = 0/*KEYEVENTF_KEYDOWN*/,
                        });
                    }

                    if ((User32.GetAsyncKeyState(0x02/*VK_RBUTTON*/) & 0x8000) == 0)
                    {
                        User32.SendInput(new User32.MOUSEINPUT
                        {
                            dwFlags = 0x08/*MOUSEEVENTF_RIGHTDOWN*/,
                        });
                    }

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MyTextBlock.Text = "[Scroll Lock] enabled";
                    }));
                }
                else
                {
                    if ((User32.GetAsyncKeyState(0x10/*VK_SHIFT*/) & 0x8000) != 0)
                    {
                        User32.SendInput(new User32.KEYBDINPUT
                        {
                            wVk = 0x10/*VK_SHIFT*/,
                            wScan = (short)User32.MapVirtualKey(0x10/*VK_SHIFT*/, 0/*MAPVK_VK_TO_VSC*/),
                            dwFlags = 0x02/*KEYEVENTF_KEYUP*/,
                        });
                    }

                    if ((User32.GetAsyncKeyState(0x02/*VK_RBUTTON*/) & 0x8000) != 0)
                    {
                        User32.SendInput(new User32.MOUSEINPUT
                        {
                            dwFlags = 0x10/*MOUSEEVENTF_RIGHTUP*/,
                        });
                    }

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MyTextBlock.Text = "[Scroll Lock] disabled";
                    }));
                }
            }
        }
    }
}
