﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using CultureInfo = System.Globalization.CultureInfo;
using Debug = System.Diagnostics.Debug;
using IEnumerable = System.Collections.IEnumerable;
using SuppressMessageAttribute = System.Diagnostics.CodeAnalysis.SuppressMessageAttribute;
using Enumerable = System.Linq.Enumerable;
using IComparer = System.Collections.IComparer;
using IEqualityComparer = System.Collections.IEqualityComparer;
using StringBuilder = System.Text.StringBuilder;
using Encoding = System.Text.Encoding;
using Interlocked = System.Threading.Interlocked;
using System.Reflection;

namespace System.Xml.Linq
{
    /// <summary>
    /// Represents an XML processing instruction.
    /// </summary>
    public class XProcessingInstruction : XNode
    {
        internal string target;
        internal string data;

        /// <summary>
        /// Initializes a new XML Processing Instruction from the specified target and string data.
        /// </summary>
        /// <param name="target">
        /// The target application for this <see cref="XProcessingInstruction"/>.
        /// </param>
        /// <param name="data">
        /// The string data that comprises the <see cref="XProcessingInstruction"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if either the target or data parameter are null.
        /// </exception>
        public XProcessingInstruction(string target, string data)
        {
            if (data == null) throw new ArgumentNullException("data");
            ValidateName(target);
            this.target = target;
            this.data = data;
        }

        /// <summary>
        /// Initializes a new XML processing instruction by copying its target and data 
        /// from another XML processing instruction.
        /// </summary>
        /// <param name="other">XML processing instruction to copy from.</param>
        public XProcessingInstruction(XProcessingInstruction other)
        {
            if (other == null) throw new ArgumentNullException("other");
            this.target = other.target;
            this.data = other.data;
        }

        internal XProcessingInstruction(XmlReader r)
        {
            target = r.Name;
            data = r.Value;
            r.Read();
        }

        /// <summary>
        /// Gets or sets the string value of this processing instruction.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the value set is null.
        /// </exception>
        public string Data
        {
            get
            {
                return data;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                bool notify = NotifyChanging(this, XObjectChangeEventArgs.Value);
                data = value;
                if (notify) NotifyChanged(this, XObjectChangeEventArgs.Value);
            }
        }

        /// <summary>
        /// Gets the node type for this node.
        /// </summary>
        /// <remarks>
        /// This property will always return XmlNodeType.ProcessingInstruction.
        /// </remarks>
        public override XmlNodeType NodeType
        {
            get
            {
                return XmlNodeType.ProcessingInstruction;
            }
        }

        /// <summary>
        /// Gets or sets a string representing the target application for this processing instruction.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the value set is null.
        /// </exception>
        public string Target
        {
            get
            {
                return target;
            }
            set
            {
                ValidateName(value);
                bool notify = NotifyChanging(this, XObjectChangeEventArgs.Name);
                target = value;
                if (notify) NotifyChanged(this, XObjectChangeEventArgs.Name);
            }
        }

        /// <summary>
        /// Writes this <see cref="XProcessingInstruction"/> to the passed in <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="XmlWriter"/> to write this <see cref="XProcessingInstruction"/> to.
        /// </param>
        public override void WriteTo(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            writer.WriteProcessingInstruction(target, data);
        }

        internal override XNode CloneNode()
        {
            return new XProcessingInstruction(this);
        }

        internal override bool DeepEquals(XNode node)
        {
            XProcessingInstruction other = node as XProcessingInstruction;
            return other != null && target == other.target && data == other.data;
        }

        internal override int GetDeepHashCode()
        {
            return target.GetHashCode() ^ data.GetHashCode();
        }

        static void ValidateName(string name)
        {
            XmlConvert.VerifyNCName(name);
            if (string.Compare(name, "xml", StringComparison.OrdinalIgnoreCase) == 0) throw new ArgumentException(SR.Format(SR.Argument_InvalidPIName, name));
        }
    }
}
