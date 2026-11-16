using System.Security.Cryptography;
using System.Text;

namespace MTBMusics.Utils;

public class Criptografia
{
    public static string GerarHash(string senha)
    {
        return BCrypt.Net.BCrypt.HashPassword(senha);
    }

    public static string gerarHash(string senha) => GerarHash(senha);

    public static bool CompararHash(string senhaInformada, string senhaBanco)
    {
        if (string.IsNullOrEmpty(senhaInformada) || string.IsNullOrEmpty(senhaBanco))
            return false;

        // Se o hash no banco for BCrypt ($2a$, $2b$, $2y$, etc.)
        if (senhaBanco.StartsWith("$2", StringComparison.Ordinal))
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(senhaInformada, senhaBanco);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                return false;
            }
        }

        // Suporte a hash SHA2_256 (como gerado no script SQL com HASHBYTES('SHA2_256', ...))
        if (senhaBanco.Length == 64)
        {
            using var sha256 = SHA256.Create();
            var bytesCalculados = sha256.ComputeHash(Encoding.UTF8.GetBytes(senhaInformada));
            var hashCalculado = Convert.ToHexString(bytesCalculados);
            if (string.Equals(hashCalculado, senhaBanco, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }

    public static bool compararHash(string senhaInformada, string senhaBanco) => CompararHash(senhaInformada, senhaBanco);
}
