import React from 'react'
import ReactDOM from 'react-dom'
import { BrowserRouter, Route } from 'react-router-dom'

const PrimaryLayout = () => (
  <div className="primary-layout">
    <header>
      Admin Our React Router 4 App
    </header>
    <main>
      <Route path="/admin/" exact component={HomePage} />
      <Route path="/admin/u" component={UsersPage} />
    </main>
  </div>
)

const HomePage =() => <div>admin Home Page</div>
const UsersPage = () => <div>admin Users Page</div>

const App = () => (
  <BrowserRouter>
    <PrimaryLayout />
  </BrowserRouter>
)

ReactDOM.render(<App />, document.getElementById('root'))