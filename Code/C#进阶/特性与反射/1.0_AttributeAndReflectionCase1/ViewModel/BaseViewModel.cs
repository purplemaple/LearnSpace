using _1._0_AttributeAndReflectionCase1.MyAttributes;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _1._0_AttributeAndReflectionCase1.ViewModel
{
    internal class BaseViewModel : BindableBase
    {
        private List<Type> heroTypes;   //保存所有英雄类的类型
        //当前选择的英雄对象

        public BaseViewModel()
        {
            heroListBox = new();
            skillListBox = new();           

            //加载所有英雄类的类型
            heroTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttribute(typeof(HeroAttribute), false) != null)
                .ToList();

            //初始化英雄列表
            heroListBox.AddRange(heroTypes.Select(t => t.Name).ToList());

            HeroCommand.Execute();
        }

        public DelegateCommand HeroCommand => new DelegateCommand(() =>
        {
            if(heroListBoxIndex == -1) return; //未选定任何项则退出

            //创建当前选择的英雄对象
            var selectedHeroType = heroTypes[heroListBoxIndex];

            //获取该英雄类型的所有技能方法
            var skillMethods = selectedHeroType.GetMethods()
                .Where(m => m.GetCustomAttribute(typeof(SkillAttribute), false) != null)
                .ToList() ;

            //初始化技能列表
            List<string> ls = skillMethods.Select(m => m.Name).ToList();
            skillListBox = ls;
        });

        public DelegateCommand SkillDoubleClickCommand => new DelegateCommand(() =>
        {
            if (skillListBoxIndex == -1) return;

            //获取当前选择的技能方法
            var selectedSkillMethod = selectedHero.GetType().GetMethod(skillListBoxItem);
            
            //调用该技能方法
            selectedSkillMethod?.Invoke(selectedHero, null);
        });

        private List<string> _heroListBox;
        public List<string> heroListBox
        {
            get => _heroListBox;
            set
            {
                SetProperty(ref _heroListBox, value);
            }
        }
        

        private int _heroListBoxIndex;
        public int heroListBoxIndex
        {
            get => _heroListBoxIndex;
            set
            {
                SetProperty(ref _heroListBoxIndex, value);
            }
        }

        private string _selectedHero;
        public string selectedHero
        {
            get => _selectedHero;
            set
            {
                SetProperty(ref _selectedHero, value);
            }
        }

        private List<string> _skillListBox;
        public List<string> skillListBox
        {
            get => _skillListBox;
            set
            {
                SetProperty(ref _skillListBox, value);
            }
        }

        private int _skillListBoxIndex;
        public int skillListBoxIndex
        {
            get => _skillListBoxIndex;
            set
            {
                SetProperty(ref _skillListBoxIndex, value);
            }
        }
        
        private string _skillListBoxItem;
        public string skillListBoxItem
        {
            get => _skillListBoxItem;
            set
            {
                SetProperty(ref _skillListBoxItem, value);
            }
        }
    }
}
