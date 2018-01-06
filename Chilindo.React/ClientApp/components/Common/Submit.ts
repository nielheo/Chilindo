const endPoint = 'http://localhost:5888'

const _submitBalance = (accountNumber: string) => {
  return fetch(endPoint + '/api/account/' + accountNumber + '/balance', {
    method: 'get',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      'Accept-Encoding': 'gzip',
      
    }
  }).then(res => {
      if (res) return res.json()
    }).catch(err => { console.log(err) })
}

const _postTransaction = (action: string, accountNumber: string, currency: string, amount: number) => {
  return fetch(endPoint + '/api/account/' +accountNumber + '/' + action, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Accept-Encoding': 'gzip',
    },
    body: JSON.stringify({
      accountNumber: accountNumber,
      currency: currency,
      amount: amount
    })
  }).then(res => {
    if (res) return res.json()
  }).catch(err => { })
}


const Submit = (action: string, accountNumber: string, currency: string = '', amount: number = 0): Promise<any> => {
  switch (action) {
    case 'balance':
      return _submitBalance(accountNumber)
    case 'deposit':
    case 'withdraw':
      return _postTransaction(action, accountNumber, currency, amount)
  }

  throw new Error('failed');
}

export default Submit