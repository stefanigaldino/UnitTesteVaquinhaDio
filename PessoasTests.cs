using FluentAssertions;
using Xunit;
using Vaquinha.Tests.Common.Fixtures;

namespace Vaquinha.Unit.Tests.DomainTests
{
    [Collection(nameof(PessoaFixtureCollection))]
    public class PessoasTests : IClassFixture<PessoaFixture>
    {
        private readonly PessoaFixture _fixture;

        public PessoasTests(PessoaFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [Trait("Pessoas", "Pessoas_PreenchidoComSusseso_PessoaValida")]
        public void Pessoas_PreenchidoComSusseso_PessoaValida()
        {
            // Arrange
            var pessoas = _fixture.PessoaValida();
            
            // Act
            var valido = pessoas.Valido();

            // Assert
            valido.Should().BeTrue(because: "os campos foram preenchidos corretamente");
            pessoa.ErrorMessages.Should().BeEmpty();            
        }

        [Fact]
        [Trait("Pessoas", "Pessoas_SemInformacoes_PessoaInvalida")]
        public void Pessoas_SemInformacoes_PessoaInvalida()
        {
            // Arrange
            var pessoas = _fixture.PessoaVazia();

            // Act
            var valido = pessoas.Valido();

            // Assert
            valido.Should().BeFalse(because: "possui erros de validação");

            pessoa.ErrorMessages.Should().HaveCount(2, because: "preencha os campos obirgatórios.");

            pessoa.ErrorMessages.Should().Contain("Nome é obrigatório.", because: "Nome não foi informado.");
            pessoa.ErrorMessages.Should().Contain("Email é obrigatório.", because: "Email não foi informado.");
        }

        [Fact]
        [Trait("Pessoas", "Pessoas_EmailInvalido_PessoaInvalida")]
        public void Pessoas_EmailInvalido_PessoaInvalida()
        {
            // Arrange
            const bool EMAIL_INVALIDO = true;
            var pessoas = _fixture.PessoaValida(EMAIL_INVALIDO);

            // Act
            var valido = pessoas.Valido();

            // Assert
            valido.Should().BeFalse(because: "Email inválido");
            pessoas.ErrorMessages.Should().HaveCount(1, because: "apenas o campo email está inválido.");

            pessoas.ErrorMessages.Should().Contain("O campo Email é inválido.");
        }

        [Fact]
        [Trait("Pessoas", "Pessoas_CamposMaxLenghtExcedidos_PessoaInvalida")]
        public void Pessoas_CamposMaxLenghtExcedidos_PessoaInvalida()
        {
            // Arrange            
            var pessoa = _fixture.PessoaMaxLenth();

            // Act
            var valido = pessoa.Valido();

            // Assert
            valido.Should().BeFalse(because: "os campos nome e email possuem mais caracteres do que o permitido.");
            pessoa.ErrorMessages.Should().HaveCount(2, because: "os dados estão inválidos.");

            pessoa.ErrorMessages.Should().Contain("O campo Nome deve possuir no máximo 150 caracteres.");
            pessoa.ErrorMessages.Should().Contain("O campo Email deve possuir no máximo 150 caracteres.");
        }
    }
}