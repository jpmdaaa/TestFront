# 🚀 AvaliacaoFrontEnd - Data Cempro

Sistema de Gerenciamento de Produtos com autenticação e integração a uma API ASP.NET.  

Interface simples, moderna e responsiva nas cores da **Data Cempro** (vermelho e branco).

---

## 📦 Tecnologias Utilizadas

- 🔸 Front-end: HTML + CSS + JavaScript puro
- 🔸 Back-end: ASP.NET Web API (.NET 6 ou superior)
- 🔸 API REST com autenticação via JWT
- 🔸 Swagger para testes da API

---

## 📂 Estrutura do Projeto

/AvaliacaoFrontEnd
├── /Frontend
│ ├── index.html # Interface web
│ ├── style.css # Estilos (vermelho e branco)
│ ├── script.js # Lógica JS (login, CRUD produtos)
│ └── /images
│ └── logo.png # Logo da Data Cempro
├── /Controllers # API Controllers
├── /Database # Configurações de Banco
├── /Models # Modelos de dados
├── /Services # Serviços da API
├── Program.cs # Inicialização do back-end
└── appsettings.json # Configurações do projeto

---

## 🔥 Como Executar

### ✅ Backend (API ASP.NET)

1. Abra a solução no **Visual Studio** ou **VS Code**.
2. Compile e execute (`F5` ou `dotnet run`).
3. A API estará disponível em:
http://localhost:5138/swagger

yaml
Copiar
Editar
4. Teste endpoints diretamente no Swagger.

---

### ✅ Frontend (Interface Web)

1. No terminal, navegue até a pasta `/Frontend`. (cd AvaliacaoFrontEnd > cd Frontend)
2. Execute:
npx serve .

*(ou use qualquer servidor estático como Live Server do VSCode)*

3. Acesse:
http://localhost:3000
*(ou na porta que aparecer)*

---

## 🏗️ Fluxo de Uso

1. Acesse a página do Frontend.
2. Informe usuário e senha (ex.: `Admin` / `123456`).
3. Após login:
   - CRUD de produtos liberado.
   - Os dados são integrados via API.
4. Clique em **Logout** para sair.

---

## 🛠️ Observações Importantes

- O CORS deve estar habilitado no backend (`Program.cs`).
- A API retorna um token JWT para autenticação.
- Se necessário, atualize o endpoint da API no arquivo `script.js` na variável:
```javascript
const API_URL = 'http://localhost:5138/api/v1';


👨‍💻 Desenvolvido por
João Pedro Macedo - Porto Alegre, RS  
Projeto de Avaliação Técnica

