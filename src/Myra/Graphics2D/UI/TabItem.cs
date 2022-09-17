﻿using System.ComponentModel;
using System;
using System.Text;
using System.Xml.Serialization;
using Myra.Attributes;
using Myra.Utility;
using Myra.MML;

#if MONOGAME || FNA
using Microsoft.Xna.Framework;
#elif STRIDE
using Stride.Core.Mathematics;
#else
using Color = FontStashSharp.FSColor;
#endif

namespace Myra.Graphics2D.UI
{
	public class TabItem : BaseObject, ISelectorItem, IContent
	{
		private string _text;
		private Color? _color;
		private ImageTextButton _button;

		public string Text
		{
			get
			{
				return _text;
			}

			set
			{
				if (value == _text)
				{
					return;
				}

				_text = value;
				FireChanged();
			}
		}

		[DefaultValue(null)]
		public Color? Color
		{
			get
			{
				return _color;
			}

			set
			{
				if (value == _color)
				{
					return;
				}

				_color = value;
				FireChanged();
			}
		}

		[Browsable(false)]
		[Content]
		public Widget Content
		{
			get; set;
		}

		[Browsable(false)]
		[XmlIgnore]
		public object Tag
		{
			get; set;
		}

		[Browsable(false)]
		[XmlIgnore]
		public IImage Image
		{
			get; set;
		}

		[Browsable(false)]
		[XmlIgnore]
		public int ImageTextSpacing
		{
			get; set;
		}

		[Browsable(false)]
		[XmlIgnore]
		internal ImageTextButton Button
		{
			get => _button;
			set
			{
				if (value == _button)
				{
					return;
				}

				if (_button != null)
				{
					_button.PressedChanged -= OnPressedChanged;
				}

				_button = value;

				if (_button != null)
				{
					_button.PressedChanged += OnPressedChanged;
				}
			}
		}

		[Browsable(false)]
		[XmlIgnore]
		public bool IsSelected
		{
			get => _button.IsPressed;
			set => _button.IsPressed = value;
		}

		public event EventHandler Changed;
		public event EventHandler SelectedChanged;

		public TabItem()
		{
		}

		public TabItem(string text, Color? color = null, object tag = null, Widget content = null)
		{
			Text = text;
			Color = color;
			Tag = tag;
			Content = content;
		}

		public TabItem(string text, Widget content): this(text, null, null, content)
		{
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			if (!string.IsNullOrEmpty(Text))
			{
				sb.Append(Text);
				sb.Append(" ");
			}

			if (!string.IsNullOrEmpty(Id))
			{
				sb.Append("(#");
				sb.Append(Id);
				sb.Append(")");
			}
			return sb.ToString();
		}

		protected internal override void OnIdChanged()
		{
			base.OnIdChanged();

			FireChanged();
		}

		protected void FireChanged()
		{
			Changed.Invoke(this);
		}

		private void OnPressedChanged(object sender, EventArgs args)
		{
			if (_button.IsPressed)
			{
				SelectedChanged.Invoke(this);
			}
		}
	}
}
