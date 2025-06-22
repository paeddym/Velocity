using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace Engine{
   public static class InputProvider {
       private static KeyboardState ?_keyboardState;
       private static FrameEventArgs _event;
       private static MouseState ?_mouseState;
       private static bool _windowFocus = false;

       public static void UpdateInputStates(KeyboardState keyboardState, FrameEventArgs even,
               MouseState mouseState, bool windowFocus) {
           _keyboardState = keyboardState;
           _event = even;
           _mouseState = mouseState;
           _windowFocus = windowFocus;
       }

       public static KeyboardState ?GetKeyboardState() {
           return _keyboardState;
       }

       public static FrameEventArgs GetFrameEvent() {
           return _event;
       }

       public static MouseState ?GetMouseState() {
           return _mouseState;
       }

       public static bool GetWindowFocus() {
           return _windowFocus;
       }
   }
}
