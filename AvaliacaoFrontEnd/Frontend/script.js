const API_URL = 'http://localhost:5138/api/v1';

const token = localStorage.getItem('token');
const loginSection = document.getElementById('loginSection');
const produtosSection = document.getElementById('produtosSection');

if (token) {
    loginSection.style.display = 'none';
    produtosSection.style.display = 'block';
    document.getElementById('logoutButton').style.display = 'inline-block';
    carregarProdutos();
} else {
    exibirLogout(false);
}


async function fazerLogin() {
    const usuario = document.getElementById('usuario').value;
    const senha = document.getElementById('senha').value;
    const loginErro = document.getElementById('loginErro');

    const response = await fetch(`${API_URL}/autenticacao/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ usuario, senha })
    });

    if (response.ok) {
        const data = await response.json();
        localStorage.setItem('token', data.access_token);
        loginSection.style.display = 'none';
        produtosSection.style.display = 'block';
        carregarProdutos();
        exibirLogout(true);

    } else {
        loginErro.innerText = 'Usuário ou senha inválidos.';
    }

    cument.getElementById('logoutButton').style.display = 'inline-block';
}

const lista = document.getElementById('listaProdutos');
const form = document.getElementById('formProduto');
const nome = document.getElementById('nome');
const valor = document.getElementById('valor');
const id = document.getElementById('produtoId');

async function carregarProdutos() {
    const token = localStorage.getItem('token');
    const response = await fetch(`${API_URL}/produtos`, {
        headers: { 'Authorization': 'Bearer ' + token }
    });

    const produtos = await response.json();
    lista.innerHTML = '';

    produtos.forEach(p => {
        const li = document.createElement('li');
        li.innerHTML = `
      <strong>${p.nome}</strong> - R$ ${p.valor.toFixed(2)}
      <button onclick="editarProduto(${p.id}, '${p.nome}', ${p.valor})">Editar</button>
      <button onclick="excluirProduto(${p.id})">Excluir</button>
    `;
        lista.appendChild(li);
    });
}

form.addEventListener('submit', async (e) => {
    e.preventDefault();
    const token = localStorage.getItem('token');

    const produto = {
        nome: nome.value,
        valor: parseFloat(valor.value)
    };

    if (id.value) {
        produto.id = parseInt(id.value);
        await fetch(`${API_URL}/produtos/${produto.id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token
            },
            body: JSON.stringify(produto)
        });
    } else {
        await fetch(`${API_URL}/produtos`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token
            },
            body: JSON.stringify(produto)
        });
    }

    form.reset();
    await carregarProdutos();
});

function editarProduto(produtoId, nomeProduto, valorProduto) {
    id.value = produtoId;
    nome.value = nomeProduto;
    valor.value = valorProduto;
}

function fazerLogout() {
    localStorage.removeItem('token');
    exibirLogout(false);
    location.reload();
}

function exibirLogout(exibir) {
    const btn = document.getElementById('logoutButton');
    btn.style.display = exibir ? 'inline-block' : 'none';
}

async function excluirProduto(produtoId) {
    const token = localStorage.getItem('token');
    await fetch(`${API_URL}/produtos/${produtoId}`, {
        method: 'DELETE',
        headers: {
            'Authorization': 'Bearer ' + token
        }
    });
    await carregarProdutos();
}
