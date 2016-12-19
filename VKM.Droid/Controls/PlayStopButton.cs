using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace VKM.Droid.Controls
{
    class PlayStopButton : ImageView
    {
        public PlayStopButton(Context context) : base(context)
        {
        }

        public PlayStopButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public PlayStopButton(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public PlayStopButton(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected PlayStopButton(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }
        private bool _isPlaying = false;
        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                _isPlaying = value;
                if (_isPlaying)
                {
                    SetImageResource(Resource.Mipmap.ic_pause_button);
                }
                else
                {
                    SetImageResource(Resource.Mipmap.ic_play_button);
                }
            }
        }
    }
}