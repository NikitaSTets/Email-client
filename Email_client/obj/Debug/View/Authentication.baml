   M S B A M L   `   `   `  ����G  CEmail_client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Email_client.ViewEmail_client.View  5   	   &     Email_client.View.Authentication   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:Email_client.View     5   	     � Title$  Authentication��5   	   $	��300��6    $	��300��6-   ���#335a75 � 	Resources 5	         MyWaterMarkStyle �� 
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
 �iii�6?   5      5      5!      5"      5#      .��5$      �  J� RenderTransformOrigin$ 0.416,0.502&�6    � ColumnDefinitions 5%   
   �� 5&      $	��53* �6   �� 5'      $
��167* �6   �� 5(      $	��72* �6    � RowDefinitions 5*   
   �� 5+      $��1* �6   �� 5,      $��1* �6   �� 5-      $��1* �6   �� 5.      $��1* �6   .+�50   
   �� -   5        $ Label��50       $��1��6   
�������$��40(�6B   $��Brodway)�6P   $��Italic$�6e   $��	UltraBold"�6x   $��Вход��6�   $
��Left=�6�   $��	31,10,0,0q�6�   $	��Top=�6�   $	��136��6�   $��58��6�   �� 51   
   -   5         ��Name$ LoginTextBox��51      $��nikit.stets@gmail.com��6)   #��[  6F   $ Login��6p   $��1��6|   $
��Left=�6�   $��23��6�   $��	10,34,0,0q�6�   $��1��6�    �� TextWrapping$
 Wrap=�6�   $	��Top=�6�   $	��147��6�   G� 52   
   -   5         G�Name$ PasswordBox��52       G� Password$ Minsk1.1.ru��6+   $��1��6B   $
��Left=�6R   $��	10,21,0,0q�6m   $��2��6�   $	��Top=�6�   $	��147��6�   �� 53   
   -   5         ��Name$ AuthetnticationButton��53      $��
Войти��6T   $��1��6d   $
��Left=�6t   $��	36,10,0,0q�6�   $��3��6�   $	��Top=�6�   $��92��6�   $��37��6�   �� 54   
   -   5        $! ResultOfAuthenticationLabel��54      $�� ��65   $��1��6@   $
��Left=�6P   $��10,8,0,0q�6k   $��1��6}   $	��Top=�6�   $	��147��6�   56      57      