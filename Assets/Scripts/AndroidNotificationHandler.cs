using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif
using UnityEngine;

public class AndroidNotificationHandler : MonoBehaviour
{
#if UNITY_ANDROID
    private const string ChannelId = "Notification_channel";
    public void ScheduleNotification(DateTime dateTime) //Schedule Notification Popup
   {
       //Every notification have to belong to a channel 
       AndroidNotificationChannel notificationChannel = new AndroidNotificationChannel
       {
           Id = ChannelId,
           Name = "Notification Cnhannel",
           Description = "Description....",
           Importance = Importance.Default
       };

       AndroidNotificationCenter.RegisterNotificationChannel(notificationChannel); //registerthe data above

       AndroidNotification notification = new AndroidNotification // create a notification
       {
           Title = "Energy Recharged!",
           Text = "Hey! Your energy has recharged, come back to play again!",
           SmallIcon = "icon_1",
           LargeIcon = "icon_2",
           FireTime = dateTime //time you want this to be sent
       };

       AndroidNotificationCenter.SendNotification(notification, ChannelId); //send notification
   }
#endif
}
