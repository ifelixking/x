import React from 'react'
import ReactDOM from 'react-dom'
import { BrowserRouter, Route, IndexRoute } from 'react-router-dom'
import Nav from './common/nav'
import HomePage from './home'
import ActorPage from './actor'

const PrimaryLayout = () => (
  <div className="primary-layout">
    <header>
      <Nav items={[
        { text: '主页', href: '/' },
        { text: '演员', href: '/actor' }
      ]} />
    </header>
    <main>
        <Route path="/" exact component={HomePage} />
        <Route path="/actor" component={ActorPage} />
    </main>
  </div>
)

const App = () => (
  <BrowserRouter>
    <PrimaryLayout />
  </BrowserRouter>
)

ReactDOM.render(<App />, document.getElementById('root'))