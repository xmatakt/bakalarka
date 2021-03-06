﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kocka.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Kocka.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330
        ///
        ///smooth in vec3 theColor;
        ///out vec4 outputColor;
        ///
        ///void main()
        ///{
        ///	outputColor = vec4(theColor, 1.0);
        ///}.
        /// </summary>
        internal static string colorScaleFrag {
            get {
                return ResourceManager.GetString("colorScaleFrag", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330
        ///
        ///uniform mat4 projectionMatrix;
        ///uniform mat4 modelViewMatrix;
        ///
        ///layout (location = 0) in vec3 inPosition;
        ///layout (location = 1) in vec3 inColor;
        ///
        ///smooth out vec3 theColor;
        ///
        ///void main()
        ///{
        ///	gl_Position = projectionMatrix*modelViewMatrix*vec4(inPosition, 1.0);
        ///	theColor = inColor;
        ///}.
        /// </summary>
        internal static string colorScaleVert {
            get {
                return ResourceManager.GetString("colorScaleVert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] fontBig {
            get {
                object obj = ResourceManager.GetObject("fontBig", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap fontBig1 {
            get {
                object obj = ResourceManager.GetObject("fontBig1", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330
        ///
        ///in vec3 TheColor;
        ///out vec4 outputColor;
        ///
        ///void main()
        ///{
        ///	outputColor=vec4(TheColor, 1.0);
        ///}.
        /// </summary>
        internal static string perFragmentShaderFrag {
            get {
                return ResourceManager.GetString("perFragmentShaderFrag", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330
        ///
        ///uniform mat4 projectionMatrix;
        ///uniform mat4 modelViewMatrix;
        ///uniform mat4 normalMatrix;
        ///uniform vec3 eye;
        ///
        ///layout (location = 0) in vec3 inPosition;
        ///layout (location = 1) in vec3 inColor;
        ///layout (location = 2) in vec3 inNormal;
        ///
        ///struct DirectionalLight
        ///{
        ///	vec3 ambient;	
        ///	vec3 diffuse;
        ///	vec3 specular;
        ///	vec3 direction;
        ///};
        ///
        ///uniform DirectionalLight light;
        ///
        ///struct Material
        ///{
        ///	float specCoef; 	//[0,1]
        ///	float diffCoef;		//[0,1]
        ///	float ambCoef;		//[0,1]
        ///	int shininess;		//[0 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string perFragmentShaderVert {
            get {
                return ResourceManager.GetString("perFragmentShaderVert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330
        ///
        ///varying vec3 diffuse,ambient,specular;
        ///varying vec3 normal;
        ///
        /////toto by som mal dostat z vertex shaderu
        ///in vec3 TheColor;
        ///in vec3 lightDirection;
        ///in vec3 EyeVector;
        ///in float shininess;
        ///
        /////posielam dalej
        /////out vec4 outputColor;
        ///
        ///void main()
        ///{
        ///	float NdotL,RdotEye;
        ///	vec3 n,R,color,ld;
        ///	
        ///	color = ambient;//nezavisle na pozicii vsetkeho
        ///
        ///	//fragment shader nemoze zapisovat varying premenne, preto n=...
        ///	n = normalize(normal);
        ///	//lightDirection = normalize(lightDirection);
        ///
        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string perPixelShaderFrag {
            get {
                return ResourceManager.GetString("perPixelShaderFrag", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330
        ///
        ///uniform mat4 projectionMatrix;
        ///uniform mat4 modelViewMatrix;
        ///uniform mat4 normalMatrix;
        ///uniform vec3 eye;
        ///
        ///varying vec3 diffuse,ambient,specular;
        ///varying vec3 normal;
        ///
        ///layout (location = 0) in vec3 inPosition;
        ///layout (location = 1) in vec3 inColor;
        ///layout (location = 2) in vec3 inNormal;
        ///
        ///struct DirectionalLight
        ///{
        ///	vec3 ambient;	
        ///	vec3 diffuse;
        ///	vec3 specular;
        ///	vec3 direction;
        ///};
        ///
        ///uniform DirectionalLight light;
        ///
        ///struct Material
        ///{
        ///	float specCoef; 	//[0,1]
        ///	float dif [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string perPixelShaderVert {
            get {
                return ResourceManager.GetString("perPixelShaderVert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330
        ///
        ///smooth in vec3 theColor;
        ///out vec4 outputColor;
        ///
        ///void main()
        ///{
        ///	outputColor = vec4(theColor, 1.0);
        ///}.
        /// </summary>
        internal static string shaderFrag {
            get {
                return ResourceManager.GetString("shaderFrag", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330
        ///
        ///uniform mat4 projectionMatrix;
        ///uniform mat4 modelViewMatrix;
        ///
        ///layout (location = 0) in vec3 inPosition;
        ///layout (location = 1) in vec3 inColor;
        ///
        ///smooth out vec3 theColor;
        ///
        ///void main()
        ///{
        ///	gl_Position = projectionMatrix*modelViewMatrix*vec4(inPosition, 1.0);
        ///	theColor = inColor;
        ///}.
        /// </summary>
        internal static string shaderVert {
            get {
                return ResourceManager.GetString("shaderVert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///// Interpolated values from the vertex shaders
        ///in vec2 UV;
        ///
        ///// Ouput data
        ///out vec4 color;
        ///
        ///// Values that stay constant for the whole mesh.
        ///uniform sampler2D gSampler;
        ///
        ///void main()
        ///{
        ///	
        ///	color = texture( gSampler, UV );//bud iba toto
        ///
        ///	//alebo toto ak chcem inverznu
        ///	//vec3 inverseCol = vec3(1.0f,1.0f,1.0f) - texture( gSampler, UV ).xyz;
        ///	//color = vec4(inverseCol,1.0f);
        ///}.
        /// </summary>
        internal static string textShaderFrag {
            get {
                return ResourceManager.GetString("textShaderFrag", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///// Input vertex data, different for all executions of this shader.
        ///layout(location = 0) in vec3 vertexPosition;
        ///layout(location = 1) in vec2 vertexUV;
        ///uniform mat4 projectionMatrix;
        ///uniform mat4 modelViewMatrix;
        ///
        ///// Output data ; will be interpolated for each fragment.
        ///out vec2 UV;
        ///
        ///void main()
        ///{
        ///	gl_Position =  projectionMatrix*modelViewMatrix*vec4(vertexPosition,1);
        ///	// UV of the vertex. No special space for this one.
        ///	UV = vertexUV;
        ///}
        ///
        ///.
        /// </summary>
        internal static string textShaderVert {
            get {
                return ResourceManager.GetString("textShaderVert", resourceCulture);
            }
        }
    }
}
