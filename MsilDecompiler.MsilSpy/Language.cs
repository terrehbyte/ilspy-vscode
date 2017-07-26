﻿// Copyright (c) 2011 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

// This is a modified version from https://github.com/icsharpcode/ILSpy/tree/v2.4/ILSpy/

using System;
using System.Collections.Generic;
using ICSharpCode.Decompiler;
using Mono.Cecil;

namespace MsilDecompiler.MsilSpy
{
    /// <summary>
    /// Base class for language-specific decompiler implementations.
    /// </summary>
    public abstract class Language
    {
        /// <summary>
        /// Gets the name of the language (as shown in the UI)
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the file extension used by source code files in this language.
        /// </summary>
        public abstract string FileExtension { get; }

        public virtual string ProjectFileExtension
        {
            get { return null; }
        }

        public virtual void DecompileMethod(MethodDefinition method, ITextOutput output, DecompilerSettings settings)
        {
            WriteCommentLine(output, TypeToString(method.DeclaringType, true) + "." + method.Name);
        }

        public virtual void DecompileProperty(PropertyDefinition property, ITextOutput output, DecompilerSettings settings)
        {
            WriteCommentLine(output, TypeToString(property.DeclaringType, true) + "." + property.Name);
        }

        public virtual void DecompileField(FieldDefinition field, ITextOutput output, DecompilerSettings settings)
        {
            WriteCommentLine(output, TypeToString(field.DeclaringType, true) + "." + field.Name);
        }

        public virtual void DecompileEvent(EventDefinition ev, ITextOutput output, DecompilerSettings settings)
        {
            WriteCommentLine(output, TypeToString(ev.DeclaringType, true) + "." + ev.Name);
        }

        public virtual void DecompileType(TypeDefinition type, ITextOutput output, DecompilerSettings settings)
        {
            WriteCommentLine(output, TypeToString(type, true));
        }

        public virtual void DecompileNamespace(string nameSpace, IEnumerable<TypeDefinition> types, ITextOutput output, DecompilerSettings settings)
        {
            WriteCommentLine(output, nameSpace);
        }

        public virtual void DecompileAssembly(AssemblyDefinition assemblyDefinition, ITextOutput output, DecompilerSettings settings)
        {
            if (assemblyDefinition != null)
            {
                var name = assemblyDefinition.Name;
                if (name.IsWindowsRuntime)
                {
                    WriteCommentLine(output, name.Name + " [WinRT]");
                }
                else
                {
                    WriteCommentLine(output, name.FullName);
                }
            }
        }

        public virtual void WriteCommentLine(ITextOutput output, string comment)
        {
            output.WriteLine("// " + comment);
        }

        /// <summary>
        /// Converts a type reference into a string. This method is used by the member tree node for parameter and return types.
        /// </summary>
        public virtual string TypeToString(TypeReference type, bool includeNamespace, ICustomAttributeProvider typeAttributes = null)
        {
            if (includeNamespace)
                return type.FullName;
            else
                return type.Name;
        }

        /// <summary>
        /// Converts a member signature to a string.
        /// This is used for displaying the tooltip on a member reference.
        /// </summary>
        public virtual string GetTooltip(MemberReference member)
        {
            if (member is TypeReference)
                return TypeToString((TypeReference)member, true);
            else
                return member.ToString();
        }

        public virtual string FormatPropertyName(PropertyDefinition property, bool? isIndexer = null)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            return property.Name;
        }

        public virtual string FormatMethodName(MethodDefinition method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            return method.Name;
        }

        public virtual string FormatTypeName(TypeDefinition type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            return type.Name;
        }

        public virtual string FormatEventName(EventDefinition @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            return @event.Name;
        }

        public virtual string FormatPropertyName(PropertyDefinition property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            return property.Name;
        }

        public virtual string FormatFieldName(FieldDefinition field)
        {
            if (field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            return field.Name;
        }

        /// <summary>
        /// Used for WPF keyboard navigation.
        /// </summary>
        public override string ToString()
        {
            return Name;
        }

        public virtual bool ShowMember(MemberReference member)
        {
            return true;
        }

        /// <summary>
        /// Used by the analyzer to map compiler generated code back to the original code's location
        /// </summary>
        public virtual MemberReference GetOriginalCodeLocation(MemberReference member)
        {
            return member;
        }

    }
}