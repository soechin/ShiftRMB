using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        bool m_pressed;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            m_func = new Winmm.TIMECALLBACK(Timer_Elapsed);
            m_timer = Winmm.timeSetEvent(100, 1, m_func, IntPtr.Zero, 1/*TIME_PERIODIC*/);
            m_pressed = false;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Winmm.timeKillEvent(m_timer);
        }

        private void Timer_Elapsed(int uTimerID, int uMsg, IntPtr dwUser, IntPtr dw1, IntPtr dw2)
        {
            short ks = User32.GetAsyncKeyState(0xA5/*VK_RMENU*/);

            if ((ks & 0x0001) != 0)
            {
                m_pressed = true;

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    MyTextBlock.Text = "Key Down";
                }));
            }
            else if ((ks & 0x8000) == 0 && m_pressed)
            {
                m_pressed = false;

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    MyTextBlock.Text = "Key Up";
                }));

                User32.SendInput(new User32.KEYBDINPUT
                {
                    wVk = 0x10/*VK_SHIFT*/,
                    wScan = (short)User32.MapVirtualKey(0x10/*VK_SHIFT*/, 0/*MAPVK_VK_TO_VSC*/),
                });

                User32.SendInput(new User32.KEYBDINPUT
                {
                    wVk = 0x02/*VK_RBUTTON*/,
                    wScan = (short)User32.MapVirtualKey(0x02/*VK_RBUTTON*/, 0/*MAPVK_VK_TO_VSC*/),
                });
            }
        }
    }
}
