This is a WinFormsMvp (https://github.com/DavidRogersDev/WinformsMVP) implementation and using XML persistency.


O principal objetivo desse projeto é demonstrar o uso do padrão MVP em uma aplicação VSTO para Microsoft Word.
A camada de visualização (View) isola-se as outras camadas, evitando a dependência do Windows Forms na camada de apresentação (Presenter).
O Presenter, por sua vez, implementa todas os handlers da view e é criado automaticamente quando uma nova instância da view é criada. 
O algoritmo de busca no conteúdo documento do Word é implementado por um serviço à parte chamado WordService.
Para a camada de Modelo (Model) foi criado um repositório chamado SearchDataXmlRepository que persiste os dados de SearchData na pasta customXml do documento Word. 
Caso seja necessário implementar um outro repositório que salve em banco de dados, JSON ou outra forma de persistência, basta implementar a interface em outro repositório.
O serviço WordService e a implementação do repositório SearchDataXmlRepository implementam o padrão Singleton, isto é, na aplicação só existirá uma instância de cada um, mesmo que existam várias classes que dependam deles. 
