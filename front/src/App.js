import { useState, useEffect } from "react";
import axios from "axios";

const API_URL = "https://localhost:7002/api/Product";

function App() {
  const [products, setProducts] = useState([]);
  const [name, setName] = useState("");
  const [price, setPrice] = useState("");

  // 取得所有商品
  const fetchProducts = async () => {
    const res = await axios.get(API_URL);
    setProducts(res.data);
  };

  // 新增商品
  const addProduct = async () => {
    if (!name || !price) return;
    await axios.post(API_URL, { name, price: parseInt(price) });
    setName("");
    setPrice("");
    fetchProducts();
  };

  // 刪除商品
  const deleteProduct = async (id) => {
    await axios.delete(`${API_URL}/${id}`);
    fetchProducts();
  };

  useEffect(() => {
    fetchProducts();  
  }, []);

  return (
    <div style={{ padding: "40px", maxWidth: "600px", margin: "0 auto" }}>
      <h1>商品管理</h1>

      {/* 新增商品 */}
      <div style={{ marginBottom: "20px" }}>
        <input
          placeholder="商品名稱"
          value={name}
          onChange={e => setName(e.target.value)}
          style={{ marginRight: "8px", padding: "6px" }}
        />
        <input
          placeholder="價格"
          value={price}
          onChange={e => setPrice(e.target.value)}
          style={{ marginRight: "8px", padding: "6px" }}
        />
        <button onClick={addProduct}>新增</button>
      </div>

      {/* 商品列表 */}
      {products.map(p => (
        <div key={p.id} style={{ 
          display: "flex", 
          justifyContent: "space-between",
          padding: "10px",
          borderBottom: "1px solid #eee"
        }}>
          <span>{p.name} - ${p.price}</span>
          <button onClick={() => deleteProduct(p.id)}>刪除</button>
        </div>
      ))}
    </div>
  );
}

export default App;