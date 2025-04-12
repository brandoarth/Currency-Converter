namespace Moedas3M
{
using Newtonsoft.Json;
using System.Net.Http;
    using Moedas3M.models;
    public partial class MainPage : ContentPage
    {
        private ExchangeRates exchangeRates;
        double valor, taxaOrigem, taxaDestino, valorConvertido;

        public MainPage()
        {
            InitializeComponent();
            CarregaPicker(moedaOrigem);
            CarregaPicker(moedaDestino);
            CarregasTaxa();
        }

        private void CarregaPicker(Picker picker)
        {
            picker.Items.Add("Real (R$) 🇧🇷");
            picker.Items.Add("Euro (€) 🇪🇺");
            picker.Items.Add("Dolar (US$) 🇺🇸");
            picker.Items.Add("Peso ($) 🇦🇷");
            picker.Items.Add("Iene (¥) 🇯🇵");
            picker.Items.Add("Dirham ( د.إ) 🇦🇪");
            picker.Items.Add("Libra Esterlina Britânica (£) 🇬🇧");
            picker.Items.Add("Bitcoin (₿) 🪙");


        }

        private async void CarregasTaxa()
        {
            indicador.IsRunning = true;
            try
            {
                var cliente = new HttpClient();
                cliente.BaseAddress = new Uri("https://openexchangerates.org");
                var url = "/api/latest.json?app_id=f490efbcd52d48ee98fd62cf33c47b9e";
                var response = await cliente.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                exchangeRates = JsonConvert.DeserializeObject<ExchangeRates>(result);
                indicador.IsRunning = false;
                CounterBtn.IsEnabled = true;
            }
            catch(Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "Ok");
                indicador.IsRunning = false;
                CounterBtn.IsEnabled = false;
                return;
            }
        }

        private void btnLimpar_Clicked(object sender, EventArgs e)
        {
            txtValor.Text = string.Empty;
            moedaOrigem.SelectedIndex = -1;
            moedaDestino.SelectedIndex = -1;
            lblMsg1.Text = string.Empty;
            lblMsg2.Text = string.Empty;
        }

        private double GetTaxa(int index)
        {
            switch (index)
            {
                case 0: return exchangeRates.rates.BRL;
                case 1: return exchangeRates.rates.EUR;
                case 2: return exchangeRates.rates.USD;
                case 3: return exchangeRates.rates.ARS;
                case 4: return exchangeRates.rates.JPY;
                case 5: return exchangeRates.rates.AED;
                case 6: return exchangeRates.rates.GBP;
                case 7: return exchangeRates.rates.BTC;

                default: return 1;

            }
        }

        async void CounterBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            valor = Convert.ToDouble(txtValor.Text);
            taxaOrigem = GetTaxa(moedaOrigem.SelectedIndex);
            taxaDestino = GetTaxa(moedaDestino.SelectedIndex);
            valorConvertido = valor / taxaOrigem * taxaDestino;
            lblMsg1.Text = string.Format("{0:N2} {1}", valor, moedaOrigem.Items[moedaOrigem.SelectedIndex]);
            lblMsg2.Text = string.Format("{0:N2} {1}", valorConvertido, moedaDestino.Items[moedaDestino.SelectedIndex]);

        }

    }

}
