using System.Diagnostics;
using OpenTK.Graphics.OpenGL;

namespace RenderEngine
{
    public static class GLCheck
    {

        // NOTE: YOU WILL NEED THIS IF YOU ARE RUNNING ON .NET FRAMEWORK 2.0
        // Comment this if you are using Framework that newer than 2.0
        public delegate void Action();

        /// <summary>
        /// Call OpenGL function and check for the error
        /// </summary>
        /// <param name="callback">OpenGL Function to be called</param>
        public static void Call(Action callback)
        {
            callback();
            CheckError();
        }

        /// <summary>
        /// Check for the OpenGL Error
        /// </summary>
        private static void CheckError()
        {
            // Note: you can add StackTrace to include file and line numbers of your error just in case for debugging purpose xD

            ErrorCode errorCode = GL.GetError();

            if (errorCode == ErrorCode.NoError)
                return;

            string error = "unknown error";
            string description = "no description";

            // Decode the error code
            switch (errorCode)
            {
                case ErrorCode.InvalidEnum:
                {
                    error = "GL_INVALID_ENUM";
                    description = "an unacceptable value has been specified for an enumerated argument";
                    break;
                }

                case ErrorCode.InvalidValue:
                {
                    error = "GL_INVALID_VALUE";
                    description = "a numeric argument is out of range";
                    break;
                }

                case ErrorCode.InvalidOperation:
                {
                    error = "GL_INVALID_OPERATION";
                    description = "the specified operation is not allowed in the current state";
                    break;
                }

                case ErrorCode.StackOverflow:
                {
                    error = "GL_STACK_OVERFLOW";
                    description = "this command would cause a stack overflow";
                    break;
                }

                case ErrorCode.StackUnderflow:
                {
                    error = "GL_STACK_UNDERFLOW";
                    description = "this command would cause a stack underflow";
                    break;
                }

                case ErrorCode.OutOfMemory:
                {
                    error = "GL_OUT_OF_MEMORY";
                    description = "there is not enough memory left to execute the command";
                    break;
                }

                case ErrorCode.InvalidFramebufferOperationExt:
                {
                    error = "GL_INVALID_FRAMEBUFFER_OPERATION_EXT";
                    description = "the object bound to FRAMEBUFFER_BINDING_EXT is not \"framebuffer complete\"";
                    break;
                }
                default:
                {
                    error = errorCode.ToString();
                    description = "";
                    break;
                }
            }

            // Log the error
            Debug.WriteLine("An internal OpenGL call failed: " + error + " (" + description + ")");
        }
    }
}
