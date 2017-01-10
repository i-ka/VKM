namespace VKM.Core.JsonClasses
{
    public class Track
    {
        public string kind { get; set; }
        public int id { get; set; }
        public string created_at { get; set; }
        public int user_id { get; set; }
        public int duration { get; set; }
        public bool commentable { get; set; }
        public string state { get; set; }
        public int original_content_size { get; set; }
        public string last_modified { get; set; }
        public string sharing { get; set; }
        public string tag_list { get; set; }
        public string permalink { get; set; }
        public bool streamable { get; set; }
        public string embeddable_by { get; set; }
        public bool downloadable { get; set; }
        public object purchase_url { get; set; }
        public object label_id { get; set; }
        public object purchase_title { get; set; }
        public string genre { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public object label_name { get; set; }
        public object release { get; set; }
        public object track_type { get; set; }
        public object key_signature { get; set; }
        public object isrc { get; set; }
        public object video_url { get; set; }
        public object bpm { get; set; }
        public int? release_year { get; set; }
        public int? release_month { get; set; }
        public int? release_day { get; set; }
        public string original_format { get; set; }
        public string license { get; set; }
        public string uri { get; set; }
        public User user { get; set; }
        public string attachments_uri { get; set; }
        public int? user_playback_count { get; set; }
        public bool user_favorite { get; set; }
        public string permalink_url { get; set; }
        public string artwork_url { get; set; }
        public string waveform_url { get; set; }
        public string stream_url { get; set; }
        public int? playback_count { get; set; }
        public int? download_count { get; set; }
        public int? favoritings_count { get; set; }
        public int? comment_count { get; set; }
    }
}