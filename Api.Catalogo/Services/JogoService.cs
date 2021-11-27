using Api.Catalogo.Entities;
using Api.Catalogo.Exceptions;
using Api.Catalogo.InputModel;
using Api.Catalogo.Repository;
using Api.Catalogo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Catalogo.Services
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _jogoRepository;

        public JogoService(IJogoRepository jogoRepository)
        {
            this._jogoRepository = jogoRepository;
        }

        public async Task Atualizar(Guid id, JogoInputModel jogo)
        {
            var entityJogo = await _jogoRepository.Obter(id);
            if (entityJogo == null)
            {
                throw new Exception();
            }

            entityJogo.Preco = jogo.Preco;
            entityJogo.Nome = jogo.Nome;
            entityJogo.Produtora = jogo.Produtora;

            await _jogoRepository.Atualizar(entityJogo);
        }

        public async Task Atualizar(Guid id, double preco)
        {
            var entityJogo = await _jogoRepository.Obter(id);
            if (entityJogo == null)
            {
                throw new JogoNaoCadastradoException();
            }

            entityJogo.Preco = preco;
            await _jogoRepository.Atualizar(entityJogo);
        }

        public async Task<JogoViewModel> Inserir(JogoInputModel jogo)
        {
            var entityJogo = await _jogoRepository.Obter(jogo.Nome, jogo.Produtora);
            if(entityJogo.Count > 0)
            {
                throw new JogoJaCadastradoException();
            }

            var jogoInsert = new Jogo
            {
                Id = Guid.NewGuid(),
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };
            await _jogoRepository.Inserir(jogoInsert);

            return new JogoViewModel
            {
                Id = jogoInsert.Id,
                Nome = jogoInsert.Nome,
                Produtora = jogoInsert.Produtora,
                Preco = jogoInsert.Preco
            };
        }

        public async Task<List<JogoViewModel>> Obter(int pagina, int quantidade)
        {
            var jogos = await _jogoRepository.Obter(pagina, quantidade);

            return jogos.Select(j => new JogoViewModel
            {
                Id = j.Id,
                Nome = j.Nome,
                Produtora = j.Produtora,
                Preco = j.Preco
            }).ToList();

        }

        public async Task<JogoViewModel> Obter(Guid id)
        {
            var jogo = await _jogoRepository.Obter(id);

            if (jogo == null)
            {
                return null;
            }

            return  new JogoViewModel
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };
        }

        public async Task Remover(Guid id)
        {
            var jogo = _jogoRepository.Obter(id);
            if (jogo == null)
            {
                throw new JogoNaoCadastradoException();
            }
            await _jogoRepository.Remover(id);
        }

        public void Dispose()
        {
            _jogoRepository?.Dispose();
        }
    }
}
