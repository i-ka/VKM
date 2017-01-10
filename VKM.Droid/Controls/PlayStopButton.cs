using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace VKM.Droid.Controls
{
    internal class PlayStopButton : ImageView
    {
        private bool _isPlaying;

        public PlayStopButton(Context context) : base(context)
        {
        }

        public PlayStopButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public PlayStopButton(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
        }

        public PlayStopButton(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected PlayStopButton(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                _isPlaying = value;
                if (_isPlaying)
                    SetImageResource(Resource.Mipmap.ic_pause_button);
                else
                    SetImageResource(Resource.Mipmap.ic_play_button);
            }
        }
    }
}