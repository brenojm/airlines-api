# 🛫 Airlines API

API ASP.NET Core + MongoDB containerizada e hospedada no Azure. Desenvolvida como parte do trabalho da disciplina SO2 da FAETERJ.

---

## 🎯 Tema Escolhido

* Tema 4: **Docker + Azure + API C#**

---

## 🔧 Tecnologias e Ferramentas

| Ferramenta   | Finalidade                         |
| ------------ | ---------------------------------- |
| ASP.NET Core | Framework da API                   |
| MongoDB      | Banco de dados NoSQL               |
| Docker       | Conteinerização da API             |
| Azure        | Hospedagem do container e banco    |
| Canva        | Criação dos slides de apresentação |

---

## 🚀 Como executar localmente

1. Clone o repositório:

   ```bash
   git clone https://github.com/brenojm/airlines-api.git
   cd airlines-api
   ```

2. Configure variáveis de ambiente

3. Build da imagem Docker:

   ```bash
   docker build -t airlines-api .
   ```

4. Execute o container:

   ```bash
   docker run -d -p 5000:80 --name mongodb mongo:latest
   docker run -d -p 5001:80 --name airlines-api --link mongodb airlines-api
   ```

5. Acesse a API:

   ```
   http://localhost:5001/api/flights
   ```

---

## 📦 Deploy no Azure

1. Build e push da imagen para um registro (Azure Container Registry)
2. Criação de um grupo de recursos e plano de App Service
3. Configuração de container (API) no Azure App Service
4. Configuração do serviço do CosmosDb como MongoDB no Azure
5. Configuração de variáveis de ambiente no portal Azure
6. Acesso à API em produção (URL do App Service)

---

## 🎥 Apresentação (exemplo)

Slides do trabalho (no Canva):
[Visualizar apresentação](https://www.canva.com/design/DAGo8eV9I-4/YdgVxOIX6xYk1Kd4-_DXcQ/edit?utm_content=DAGo8eV9I-4&utm_campaign=designshare&utm_medium=link2&utm_source=sharebutton)

---

## 💡 Instruções do Trabalho

* Implementação individual ou em dupla
* Contexto exclusivo: cada tema só pode ser usado por um grupo
* Plágio resulta em nota zerada
* Presença obrigatória nas apresentações; participação conta ponto

---

## 📝 Contato

Desenvolvedor: **Breno J. M.**
Email ou contato pelo GitHub
