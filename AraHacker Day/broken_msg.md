> CTF AraHacker Day

> Em outubro de 2017 rolou o primeiro AraHacker Day, evento organizado pela Legião HC de Arapiraca/AL para  levar o tema segurança cibernética, hacker, maker e afins para comunidade da região. O Legião HC convidou o time do RATF (Rage Against the Flag) para que organizassem o CTF do evento. O CTF teve um nível iniciante para que o pessoal pudesse conhecer e ser introduzido para esse tipo de competicao. 

> broken msg: “Mostre seu conhecimento em análise de pacotes e encontre a flag”

O desafio nos fornece um arquivo com a extensão pcapng, vamos conferir:
´´´shell
$ file net.pcapng
net.pcapng: pcap-ng capture file – version 1.0
´´´

Abri o arquivo com o wireshark, ao analisar os primeiros pacotes TCP com 'tcp follow' encontramos uma trollagem. Extraio o conteúdo do pcap com o tcpflow para verificar todos os dados que o tcpflow consiga extrair:
´´´shell
tcpflow -r net.pcapng
´´´

Ao exibir o conteúdo dos arquivos encontramos a trollagem e o que parece ser três pedaços de um hash, juntando e decodando em base32 obtenho a flag.

´´´shell
$ cat 192.168.001.19*
tranquilidade
procura aew man kkk
aew blz
e a flag...
NYZXI5ZQ
OJVV6MZU
ON4Q====

$ echo "NYZXI5ZQOJVV6MZUON4Q====" | base32 -d
*******_****
´´´


Obrigado!