   M S B A M L   `   `   `  ����Q  MStudentsProjectChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:StudentsProjectCheckerStudentsProjectChecker  5   	   )    #StudentsProjectChecker.Registration   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6local$clr-namespace:StudentsProjectChecker     5   	     � Title$  Регистрация��5   	   $	��359��6   $	��607��6*   .��5	      � $��0,0,0,-6q�6   .+�5
   
   � $��Center=�6   $	��320��6,   $��0,12,0,0q�69   $	��Top=�6K   $	��475��6c   .+�5      	� -   5         	�Name$ teacherPicked��5      $ ��Преподаватель��61   $
��Left=�6I   $��	10,25,0,0q�6d   $	��Top=�6w   6�   	� 5      -   5        $ studentPicked��5      $*��$Студент                      ��61   $
��Left=�6Y   $��
166,25,0,0q�6t   $	��Top=�6�   6�   �� 5      -   5         ��Name$ LastName��5      $
��Left=�6(   $��4,70,0,0q�6C   $�� ��6U    �� TextWrapping$
 Wrap=�6]   $	��Top=�6q   $	��139��6�   $��14(�6�   $��Times New Roman)�6�   �� 5      $��Фамилия��6   $
��Left=�6&   $��1,45,0,0q�6A   $	��Top=�6S   $��11(�6k   ���	#FF7C5B5B6y   �� 5      -   5        $ 
patronymic��5      $
��Left=�6*   $��
335,70,0,0q�6E   $�� ��6Y   $
 Wrap=�6a   $	��Top=�6u   $	��138��6�   $��14(�6�   $��Times New Roman)�6�   �� 5      $��Имя��6   $
��Left=�6"   $��
161,45,0,0q�6=   $	��Top=�6Q   $��11(�6i   ���	#FF7C5B5B6w   �� 5      -   5        $ 	FirstName��5      $
��Left=�6)   $��
166,70,0,0q�6D   $�� ��6X   $
 Wrap=�6`   $	��Top=�6t   $	��139��6�   $��14(�6�   $��Times New Roman)�6�   �� 5      $��Отчество��6   $
��Left=�6'   $��
342,45,0,0q�6B   $	��Top=�6V   $��11(�6n   ���	#FF7C5B5B6|   �� 5      $��Группа��6   $
��Left=�6%   $��1,98,0,0q�6@   $	��Top=�6R   $��11(�6j   ���	#FF7C5B5B6x   �� 5      -   5         ��Name$ 	groupList��5      $
��Left=�6*   $��	4,123,0,0q�6E   $	��Top=�6X   $	��139��6p   6|   �� 5      -   5        $ EMailtxt��5      $
��Left=�6(   $��167,125,0,0q�6C   $�� ��6X   $
 Wrap=�6`   $	��Top=�6t   $	��138��6�   $��14(�6�   $��Times New Roman)�6�   �� 5      $��
Почта��6   $
��Left=�6$   $��
161,98,0,0q�6?   $	��Top=�6S   $��11(�6k   ���	#FF7C5B5B6y   �� 5      $��Пароль��6   $
��Left=�6%   $��334,100,0,0q�6@   $	��Top=�6U   $��11(�6m   ���	#FF7C5B5B6{   G� 5      -   5         G�Name$ pwdTxt��5      $
��Left=�6*   $��335,127,0,0q�6E   $	��Top=�6Z   $	��140��6r   $��14(�6~   $��Times New Roman)�6�   �� 5      $%��Список предметов��6   $
��Left=�6/   $��	6,150,0,0q�6J   $	��Top=�6]   $��11(�6u   ���	#FF7C5B5B6�   �� 5      $(��"Тип учетной записи��6   $
��Left=�61   $	��Top=�6L   $��11(�6d   ���	#FF7C5B5B6r   �� 5      -	   5         ��Name$ registerBtn��5      $*��$Зарегистрироваться��6*   $
��Left=�6G   $��335,173,0,0q�6b   $	��Top=�6w   $	��119��6�   5      &   System.Windows.Controls.DataGrid  5   
   -
   5          Name$ subjectGrid��5      $��77,186,229,31q�6(   5!      5"      