   M S B A M L   `   `   `  ����G  CEmail_client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Email_clientEmail_client  5   	   "    Email_client.View.SmtpWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:Email_client     5   	     � Title$  Email client��5   	   $��526.684��6   $��349.826��6/    � 	Resources 5	         MyWaterMarkStyle �� 
TargetType �� Property �� Value �� 
TargetType ��Name ��Name
 � Tag	 �� Triggers
 ]� 
Conditions �� Property �� Value �� 
TargetName P� Property P� Value��%�  &	        �� 5
   
   " ��6)   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      � .+�5      �� 
����������	#FF7D8683$��1q�6P   �� 5      $ PART_ContentHost��6+   $��5,0,0,0q�6E   $��Center=�6V   �� 5      $ WaterMarkLabel��65   $��5,0,0,0q�6$   #��z 6M   $��Center=�6m   $y�	Collapsed=�5       
�������$��Italic)�6I   5      	 5      ]� 5      
 5   "   �� 5   &    � ��$  ��6@   ./�5   "   �� $ WaterMarkLabel��6?    � y�$ Visible=�6[   5      P� 5       � {� .  . �5   "   ��  � ��
 �iii�6?   5      5      5!      5"      5#      .��5$      � ���#335a75$��0,0,2,0q�6    .+�5%   
   �� -   5         ��Name$ 
ToGroupBox��5%      $��To��6'   
�������$
��Left=�6F   $��	10,10,0,0q�6a   $	��Top=�6t   $��84��6�   $	��313��6�   .��5&      � $
��Left=�6   $��66��6.   $��	0,0,-2,-4q�6:   $	��Top=�6M   $	��303��6e   .+�5'      �� -   5        $ ToEmailLabel��5'   +   
�������$��E-mail:��6A   $
��Left=�6S   $��	10,19,0,0q�6n   $	��Top=�6�   �� 5(      -   5         ��Name$ ToEmailTextBox��5(      #��[  62   $ Input email��6\   $
��Left=�6n   $��23��6�   $��87,19,-49,0q�6�    �� TextWrapping$
 Wrap=�6�   $	��Top=�6�   $	��206��6�   5)      5*      �� 5+   
   $
��Left=�6   $	��232��60   $��	13,98,0,0q�6=   $	��Top=�6P   $	��308��6h    J� RenderTransformOrigin$ 0.5,0.5&�6t   z�5,      V� 5-      .~�5.      �� �� 5/      �� 50       �� Angle$ -0.174Q�6&   T� 51      52      .+�54      �� -   5        $ 
TopicLabel��54      
�������$��Topic��6;   $��29��6K   �� 55      -   5        $ TopicTextBox��55      #��[  6,   $ Input topic��6V   $��23��6h   $
 Wrap=�6t   �� 56      -   5        $ ContentOfMessgeLabel��56      
�������$��Content of message��6E   �� 57      -   5        $# ContentOfMessageSubHeaderText��57      $	��150��58      $��20(�59      $
 Wrap=�5:      
���   � �� AcceptsReturn . 5<      5>      �� 5?   
   -   5         ��Name$ 
SendButton��5?      $
��Send��6%   $
��Left=�64   $��
30,399,0,0q�6O   $	��Top=�6c   $	��120��6{   $��56��6�   6�   �� 5@   
   -	   5        $ CancelButton��5@      $��Cancel��6'   $
��Left=�68   $��203,399,0,0q�6S   $	��Top=�6h   $	��120��6�   $��56��6�   6�   5A      5B      