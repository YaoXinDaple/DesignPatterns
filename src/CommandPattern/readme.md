## 命令模式

### 定义
命令模式（Command Pattern）是一种行为设计模式，它将请求封装为一个对象，从而使您可以使用不同的请求、队列或日志请求，以及支持可撤销操作。

### 适用场景
- 当一个对象有固定的操作，但需要将这些操作封装为对象时，可以使用命令模式。（例如智能灯泡的遥控器，有开/关/调亮/调暗按钮，当用户需要自定义按钮的操作时，就适合将用户自定义的命令作为对象传入一个实例化的遥控器类中。）

比如，我们还有可能要支持一个遥控器同时操作多个灯泡，或者关闭是可以同时关闭所有灯泡。或者两个遥控器各控制一半的灯泡。 