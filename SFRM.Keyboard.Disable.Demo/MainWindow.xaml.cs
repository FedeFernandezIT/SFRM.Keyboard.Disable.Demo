using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SFRM.Keyboard.Disable.Demo
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LowLevelKeyboardProc hook = null;
        private bool firstStroke = false;
        private IntPtr hHook;
        private string keyStored;
        private IntPtr hModule;

        public MainWindow()
        {
            InitializeComponent();
            hook = new LowLevelKeyboardProc(MyCallbackFunction);
            hModule = GetModuleHandle(null);            
        }

        private IntPtr MyCallbackFunction(int code, IntPtr wParam, IntPtr lParam)
        {                     
            return (IntPtr)1;            
            return CallNextHookEx(hHook, code, wParam, lParam);            
        }
                
        #region DLLs Imported
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetWindowsHookEx(int hookType, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        delegate IntPtr LowLevelKeyboardProc(int code, IntPtr wParam, IntPtr lParam);

        private const int HC_ACTION = 0;
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);        
        #endregion

        private void Window_Activated(object sender, EventArgs e)
        {
            hHook = SetWindowsHookEx(WH_KEYBOARD_LL, hook, hModule, 0);
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            UnhookWindowsHookEx(hHook);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }
    }
}
